using System.Collections.Generic;
using UnityEngine;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class ValueLinkedManipulation
    {
        public string name;
        public List<Transform> targets = new();

        public ManipulationType manipulationType = ManipulationType.Rotate;
        public Axis axis = Axis.Y;

        [Header("Value Range")]  public float min, max = 90;
        [Header("Easing Curve")] public AnimationCurve responseCurve = AnimationCurve.Linear(0,0,1,1);

        public void Apply(float n)
        {
            float tCurve = responseCurve != null ? responseCurve.Evaluate(Mathf.Clamp01(n)) : n;
            float v      = Mathf.Lerp(min, max, tCurve);

            foreach (var t in targets)
            {
                if (!t) continue;

                switch (manipulationType)
                {
                    case ManipulationType.Move:
                        var p = t.localPosition;
                        AxisHelper.SetAxes(ref p, axis, v);
                        t.localPosition = p;
                        break;

                    case ManipulationType.Rotate:
                        var r = t.localEulerAngles;
                        AxisHelper.SetAxes(ref r, axis, v);
                        t.localEulerAngles = r;
                        break;

                    case ManipulationType.Scale:
                        var s = t.localScale;
                        AxisHelper.SetAxes(ref s, axis, v);
                        t.localScale = s;
                        break;
                }
            }
        }
    }
}