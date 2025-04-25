using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class HoverTransformPreset
    {
        public string name;
        public List<Transform> targets = new();

        public ManipulationType manipulationType = ManipulationType.Move;
        public Axis axis = Axis.Y;
        public float amount = 1;

        [Header("Timing")]
        public float  delay      = 0;        // delay before first ease‑in AND valley wait
        public float  duration   = .25f;
        public float  waitAtPeak = .1f;

        public bool   oscillate  = false;
        public bool   worldSpace = false;

        [Header("Easing")]
        public Ease easeIn  = Ease.OutQuad;
        public Ease easeOut = Ease.InQuad;

        // ------------------------------------------------------------------
        public void Apply(Interactivate host, bool entering)
        {
            foreach (var t in targets)
            {
                if (!t) continue;
                DOTween.Kill(t);

                switch (manipulationType)
                {
                    case ManipulationType.Move:   DoMove  (t, host, entering); break;
                    case ManipulationType.Rotate: DoRotate(t, host, entering); break;
                    case ManipulationType.Scale:  DoScale (t, host, entering); break;
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        void DoMove(Transform tr, Interactivate host, bool entering)
        {
            Vector3 delta = AxisHelper.Mask(axis, amount);

            if (oscillate && entering)
            {
                Vector3 basePos = worldSpace ? host.GetBaseWorldPos(tr) : host.GetBaseLocalPos(tr);

                var seq = DOTween.Sequence().SetTarget(tr);
                if (delay > 0) seq.AppendInterval(delay);

                if (worldSpace)
                     seq.Append(tr.DOMove(basePos + delta, duration).SetEase(easeIn));
                else seq.Append(tr.DOLocalMove(basePos + delta, duration).SetEase(easeIn));

                seq.AppendInterval(waitAtPeak);

                if (worldSpace)
                     seq.Append(tr.DOMove(basePos, duration).SetEase(easeOut));
                else seq.Append(tr.DOLocalMove(basePos, duration).SetEase(easeOut));

                seq.AppendInterval(delay);          // valley wait
                seq.SetLoops(-1);
                host.TrackHoverTween(seq);
            }
            else
            {
                float dir = entering ? 1 : -1;
                Tween tw = TweenExtensions.BuildTween(tr, ManipulationType.Move, axis, amount * dir,
                                                      duration, entering ? easeIn : easeOut,
                                                      entering ? delay : 0,
                                                      0, LoopType.Restart, 0, worldSpace);
                host.TrackHoverTween(tw);
            }
        }

        void DoRotate(Transform tr, Interactivate host, bool entering)
        {
            Vector3 delta = AxisHelper.Mask(axis, amount);

            if (oscillate && entering)
            {
                Vector3 baseRot = worldSpace ? host.GetBaseWorldRot(tr) : host.GetBaseLocalRot(tr);

                var seq = DOTween.Sequence().SetTarget(tr);
                if (delay > 0) seq.AppendInterval(delay);

                if (worldSpace)
                     seq.Append(tr.DORotate(baseRot + delta, duration, RotateMode.FastBeyond360).SetEase(easeIn));
                else seq.Append(tr.DOLocalRotate(baseRot + delta, duration, RotateMode.FastBeyond360).SetEase(easeIn));

                seq.AppendInterval(waitAtPeak);

                if (worldSpace)
                     seq.Append(tr.DORotate(baseRot, duration, RotateMode.FastBeyond360).SetEase(easeOut));
                else seq.Append(tr.DOLocalRotate(baseRot, duration, RotateMode.FastBeyond360).SetEase(easeOut));

                seq.AppendInterval(delay);
                seq.SetLoops(-1);
                host.TrackHoverTween(seq);
            }
            else
            {
                float dir = entering ? 1 : -1;
                Tween tw = TweenExtensions.BuildTween(tr, ManipulationType.Rotate, axis, amount * dir,
                                                      duration, entering ? easeIn : easeOut,
                                                      entering ? delay : 0,
                                                      0, LoopType.Restart, 0, worldSpace);
                host.TrackHoverTween(tw);
            }
        }

        void DoScale(Transform tr, Interactivate host, bool entering)
        {
            Vector3 baseScale = host.GetBaseScale(tr);
            Vector3 delta = AxisHelper.Mask(axis, amount);

            if (oscillate && entering)
            {
                var seq = DOTween.Sequence().SetTarget(tr);
                if (delay > 0) seq.AppendInterval(delay);

                seq.Append(tr.DOScale(baseScale + delta, duration).SetEase(easeIn));
                seq.AppendInterval(waitAtPeak);
                seq.Append(tr.DOScale(baseScale, duration).SetEase(easeOut));
                seq.AppendInterval(delay);
                seq.SetLoops(-1);
                host.TrackHoverTween(seq);
            }
            else
            {
                Vector3 target = entering ? baseScale + delta : baseScale;
                Tween tw = tr.DOScale(target, duration)
                             .SetEase(entering ? easeIn : easeOut)
                             .SetDelay(entering ? delay : 0);
                host.TrackHoverTween(tw);
            }
        }
    }
}