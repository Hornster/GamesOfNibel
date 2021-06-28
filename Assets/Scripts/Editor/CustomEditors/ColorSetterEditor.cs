using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.CustomEditors
{
    [CustomEditor(typeof(ColorSetter))]
    public class ColorSetterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var colorSetterScript = target as ColorSetter;
            colorSetterScript.OverrideAlpha = EditorGUILayout.Toggle("Override Alpha channel", colorSetterScript.OverrideAlpha);

            if (colorSetterScript.OverrideAlpha)
            {
                colorSetterScript.AlphaOverrideValue =
                    EditorGUILayout.FloatField("Alpha value", colorSetterScript.AlphaOverrideValue);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(colorSetterScript);
            }
        }
    }
}
