#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Nightbyte.Interactivate
{
    [CustomPropertyDrawer(typeof(InputAxisAttribute))]
    public class InputAxisDrawer : PropertyDrawer
    {
        string[] _axes;

        string[] GetAxes()
        {
            if (_axes != null) return _axes;

            var asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset");
            if (asset != null && asset.Length > 0)
            {
                var so       = new SerializedObject(asset[0]);
                var axesProp = so.FindProperty("m_Axes");
                var list     = new List<string>();

                for (int i = 0; i < axesProp.arraySize; i++)
                {
                    var elem  = axesProp.GetArrayElementAtIndex(i);
                    var nameP = elem.FindPropertyRelative("m_Name");
                    if (!string.IsNullOrEmpty(nameP.stringValue))
                        list.Add(nameP.stringValue);
                }
                _axes = list.ToArray();
            }

            if (_axes == null || _axes.Length == 0)
                _axes = new[] { "Horizontal", "Vertical", "Mouse X", "Mouse Y" };
            return _axes;
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            var axes = GetAxes();
            int idx  = System.Array.IndexOf(axes, prop.stringValue);
            if (idx < 0) idx = 0;

            idx = EditorGUI.Popup(pos, label.text, idx, axes);
            prop.stringValue = axes[idx];
        }
    }
}
#endif