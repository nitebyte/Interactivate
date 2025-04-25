using UnityEngine;
using DG.Tweening;

namespace Nightbyte.Interactivate
{
    public static class TweenExtensions
    {
        public static Tween BuildTween(Transform t,ManipulationType m,Axis ax,float d,
                                       float dur,Ease e,float del,
                                       int loops,LoopType lt,
                                       float loopDelay = 0,bool worldSpace = false)
        {
            var vMask = AxisHelper.Mask(ax,d);

            Tween core = m switch
            {
                ManipulationType.Move   => worldSpace ? t.DOMove  (vMask,dur).SetRelative()
                                                      : t.DOLocalMove(vMask,dur).SetRelative(),
                ManipulationType.Rotate => worldSpace ? t.DORotate(vMask,dur,RotateMode.FastBeyond360).SetRelative()
                                                      : t.DOLocalRotate(vMask,dur,RotateMode.FastBeyond360).SetRelative(),
                _                       => null
            };
            if (core == null) return null;

            core.SetEase(e);

            if (loopDelay > 0)
            {
                var seq = DOTween.Sequence();
                seq.Append(core);
                seq.AppendInterval(loopDelay);
                return seq.SetDelay(del).SetLoops(loops,lt);
            }
            return core.SetDelay(del).SetLoops(loops,lt);
        }

        public static float NormaliseAngle(float a) => a > 180 ? a - 360 : a;
    }
}