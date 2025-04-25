using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class ObjectManipulationPreset
    {
        public string name;
        public List<Transform> targets = new();

        public ManipulationType manipulationType = ManipulationType.Move;
        public Axis axis = Axis.Y;
        public float amount = 1;

        [Header("Clamp")] public bool clamp; public float min, max = 1;

        [Header("Tween")]
        public float duration = 1, delay;
        public Ease  ease = Ease.Linear;
        public int   loops;
        public LoopType loopType;
        public float loopDelay;
        public bool  useWorldSpace;

        [Header("Target State Change")]
        public bool  enableTargets;      public float enableDelay;
        public bool  disableTargets;     public float disableDelay;
        public bool  destroyInsteadOfDisable;

        public void Execute()
        {
            bool multiAxis = axis >= Axis.XY;

            foreach (var t in targets)
            {
                if (!t) continue;

                switch (manipulationType)
                {
                    case ManipulationType.Move:
                    case ManipulationType.Rotate:
                    {
                        float delta = amount;

                        if (!multiAxis && clamp)
                        {
                            var ar  = AxisHelper.AX(axis);
                            float cur = manipulationType == ManipulationType.Move
                                ? (useWorldSpace ? ar.Get(t.position) : ar.Get(t.localPosition))
                                : (useWorldSpace ? TweenExtensions.NormaliseAngle(ar.Get(t.eulerAngles))
                                                 : TweenExtensions.NormaliseAngle(ar.Get(t.localEulerAngles)));

                            float tgt  = Mathf.Clamp(cur + amount, min, max);
                            delta      = tgt - cur;
                            if (Mathf.Approximately(delta,0)) break;
                        }

                        TweenExtensions.BuildTween(t, manipulationType, axis, delta,
                                                   duration, ease, delay,
                                                   loops, loopType, loopDelay, useWorldSpace);
                        break;
                    }

                    case ManipulationType.Scale:
                    {
                        var newScale = t.localScale;
                        AxisHelper.SetAxes(ref newScale, axis,
                                           clamp ? Mathf.Clamp(amount, min, max) : amount);

                        if (Vector3.Distance(t.localScale, newScale) < 1e-4f) break;

                        var core = t.DOScale(newScale, duration).SetEase(ease);

                        if (loopDelay > 0)
                        {
                            var seq = DOTween.Sequence();
                            seq.Append(core);
                            seq.AppendInterval(loopDelay);
                            seq.SetDelay(delay).SetLoops(loops, loopType);
                        }
                        else core.SetDelay(delay).SetLoops(loops, loopType);
                        break;
                    }
                }
            }

            if (enableTargets)
                DOVirtual.DelayedCall(enableDelay, () =>
                {
                    foreach (var tr in targets) if (tr) tr.gameObject.SetActive(true);
                });

            if (disableTargets || destroyInsteadOfDisable)
                DOVirtual.DelayedCall(disableDelay, () =>
                {
                    foreach (var tr in targets)
                        if (tr)
                            if (destroyInsteadOfDisable) Object.Destroy(tr.gameObject);
                            else                         tr.gameObject.SetActive(false);
                });
        }
    }
}