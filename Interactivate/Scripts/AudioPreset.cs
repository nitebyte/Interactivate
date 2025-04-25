using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class AudioPreset
    {
        public string name;
        public List<AudioClip> clips = new();
        public bool cutOthers, loop;
        public int  loopCount = 1;
        [Range(0,1)] public float volume = 1;
        public float delay, fadeIn, fadeOut;

        public void Execute(GameObject host)
        {
            if (clips.Count == 0) return;

            var src = host.GetComponent<AudioSource>() ?? host.AddComponent<AudioSource>();
            if (cutOthers)
                foreach (var s in Object.FindObjectsOfType<AudioSource>())
                    if (s != src) s.Stop();

            var clip = clips[Random.Range(0, clips.Count)];

            DOVirtual.DelayedCall(delay, () =>
            {
                src.clip = clip;
                src.volume = 0;
                src.loop = loop;
                src.Play();

                if (fadeIn > 0) DOTween.To(() => src.volume, v => src.volume = v, volume, fadeIn);
                else             src.volume = volume;

                float dur = !loop ? clip.length
                                  : (loopCount > 0 ? clip.length * loopCount : 0);

                if (dur > 0)
                    DOVirtual.DelayedCall(dur, () =>
                    {
                        if (fadeOut > 0)
                            DOTween.To(() => src.volume, v => src.volume = v, 0, fadeOut)
                                   .OnComplete(src.Stop);
                        else src.Stop();
                    });
            });
        }
    }
}