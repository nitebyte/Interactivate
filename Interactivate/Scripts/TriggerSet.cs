using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class TriggerSet
    {
        [Header("Identification")] public string setName;

        // ───────── Input ─────────
        [System.Serializable]
        public class InputSec
        {
            public InputMode inputMode = InputMode.Button;
            public KeyCode activationKey = KeyCode.E;
            public ButtonActivationMode buttonActivation = ButtonActivationMode.OnKeyDown;

            [Header("Button")] public int increment = 1; public bool fireContinuous;
            [Header("Hold")]   public float holdDuration = 1; [HideInInspector] public float holdTimer; [HideInInspector] public bool holdFired;
            [Header("Toggle")] public bool toggleState; public int toggleOnValue = 1, toggleOffValue;

            [Header("Axis")]
            [InputAxis] public string axisName = "Mouse ScrollWheel";
            public bool  invertAxis;
            public int   axisIncrement = 1;
            [HideInInspector] public float axisBuf;
        }

        // ───────── Value ─────────
        [System.Serializable] public class ValueSec { public bool useLimits = true; public int intValue, minValue, maxValue = 10; }

        // ───────── Usage ─────────
        [System.Serializable]
        public class UsageSec
        { public int maxUses = 1; public float resetTime; public bool removeOnExpire, destroyOnExpire; [HideInInspector] public int currentUses; [HideInInspector] public float lastUse; }

        public InputSec input = new(); public ValueSec value = new(); public UsageSec usage = new();

        [Header("Value‑Linked")]           public List<ValueLinkedManipulation> valueLinked       = new();
        [Header("Triggered Manipulations")]public List<ObjectManipulationPreset> manipulationPresets = new();
        [Header("FX / Audio / Tag")]       public List<FXPreset>   fxPresets    = new();
                                            public List<AudioPreset> audioPresets = new();
                                            public List<TextManipulationSet> textPresets = new();
                                            public List<TagLayerChange> tagLayerChanges = new();
        public UnityEvent onActivateEvents;

        public bool Maxed => usage.maxUses > 0 && usage.currentUses >= usage.maxUses;

        public void Set (int v) => value.intValue = value.useLimits ? Mathf.Clamp(v, value.minValue, value.maxValue) : v;
        public void Add (int d) => Set(value.intValue + d);

        public void AddAxis(float d)
        {
            input.axisBuf += d;
            int steps = Mathf.FloorToInt(Mathf.Abs(input.axisBuf));
            if (steps == 0) return;

            int sign = input.axisBuf > 0 ? 1 : -1;
            Add(steps * sign);
            input.axisBuf -= steps * sign;
        }

        public float Norm => !value.useLimits || value.maxValue == value.minValue
                             ? 0
                             : Mathf.InverseLerp(value.minValue, value.maxValue, value.intValue);

        public void ResetHold()     { input.holdTimer = 0; input.holdFired = false; }
        public void TryAutoReset()
        {
            if (usage.resetTime > 0 && usage.currentUses > 0 && Time.time - usage.lastUse >= usage.resetTime)
                usage.currentUses = 0;
        }
        public void ApplyValueLinked() { float t = Norm; valueLinked.ForEach(v => v.Apply(t)); }
    }
}