using System.Collections.Generic;
using UnityEngine;

namespace Nightbyte.Interactivate
{
    [System.Serializable]
    public class TagLayerChange
    {
        public string name;
        public List<GameObject> targets = new();

        public bool changeTag;   public string newTag = "";
        public bool changeLayer; public int    newLayer;

        public void Execute()
        {
            foreach (var go in targets)
            {
                if (!go) continue;
                if (changeTag  && !string.IsNullOrEmpty(newTag)) go.tag   = newTag;
                if (changeLayer)                                go.layer = newLayer;
            }
        }
    }
}