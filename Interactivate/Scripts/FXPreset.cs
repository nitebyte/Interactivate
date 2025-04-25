using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class FXPreset
    {
        public string name;
        public List<GameObject> fxObjects = new();
        public bool activate = true, deactivate;
        public float delay;

        public void Execute()
        {
            foreach (var g in fxObjects)
            {
                if (!g) continue;
                if (activate)   DOVirtual.DelayedCall(delay, () => g.SetActive(true));
                if (deactivate) DOVirtual.DelayedCall(delay, () => g.SetActive(false));
            }
        }
    }
}