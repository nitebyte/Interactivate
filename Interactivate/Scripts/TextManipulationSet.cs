using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class TextManipulationSet
    {
        public string name;
        public List<TMP_Text> targets = new();

        [Header("Text")]  public bool changeText;  public string newText = "";  public float textDur = 1;  public ScrambleMode scramble;
        [Header("Color")] public bool changeColor;public Color newColor = Color.white; public float colDur = 1;
        [Header("Font")]  public bool changeFont; public float newSize = 36;  public float sizeDur = 1;
        [Header("Tween")] public float delay; public Ease ease = Ease.Linear;

        public void Execute()
        {
            foreach (var t in targets)
            {
                if (!t) continue;

                if (changeText)
                {
                    var tw = DOTween.To(() => t.text, x => t.text = x, newText, textDur)
                                    .SetDelay(delay).SetEase(ease);
                    if (tw is TweenerCore<string,string,StringOptions> sc)
                        sc.SetOptions(true, scramble, null);
                }

                if (changeColor)
                    DOTween.To(() => t.color, c => t.color = c, newColor, colDur)
                           .SetDelay(delay).SetEase(ease);

                if (changeFont)
                    DOTween.To(() => t.fontSize, s => t.fontSize = s, newSize, sizeDur)
                           .SetDelay(delay).SetEase(ease);
            }
        }
    }
}