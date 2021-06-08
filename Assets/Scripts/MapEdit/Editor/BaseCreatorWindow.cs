using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.MapEdit.Common.Data.ScriptableObjects;
using Assets.Scripts.MapEdit.Editor.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor
{
    public class BaseCreatorWindow : EditorWindow
    {
        private BaseMarkerFactorySO _baseMarkerFactorySo;
        private bool _requiredRefsGroupFoldout = false;

        [MenuItem(SGMapEditPaths.WindowsPath + "/Base Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<BaseCreatorWindow>("Base Creator Window");
            window.FindBaseMarkerFactorySo();
        }

        private void OnGUI()
        {
            CreateRequiredReferencesGroup();
            GUILayout.Label("TODO", EditorStyles.boldLabel);
        }

        private void CreateRequiredReferencesGroup()
        {
            _requiredRefsGroupFoldout = EditorGUILayout.Foldout(_requiredRefsGroupFoldout, "Required references");
            if (_requiredRefsGroupFoldout)
            {
                var previousFoldout = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;

                GUILayout.TextField("None of these should be null/empty.");
                _baseMarkerFactorySo = EditorGUILayout.ObjectField("Factory object", _baseMarkerFactorySo, typeof(BaseMarkerFactorySO), false) as BaseMarkerFactorySO;

                EditorGUI.indentLevel = previousFoldout;
            }

        }

        private void FindBaseMarkerFactorySo()
        {
            var factoryFileName = BaseMarkerFactorySO.BaseMarkerFactorySoName;
            var hitsGUIDs = AssetDatabase.FindAssets(factoryFileName, new[] { BaseMarkerFactorySO.PathToBaseMarkerFactorySo });

            if (hitsGUIDs.Length <= 0)
            {
                throw new Exception($"Cannot find Base Marker Factory SO at {BaseMarkerFactorySO.PathToBaseMarkerFactorySo}!");
            }
            else if (hitsGUIDs.Length > 1)
            {
                throw new Exception($"Only one Base Marker Factory SO is allowed at {BaseMarkerFactorySO.PathToBaseMarkerFactorySo}!");
            }

            var pathToFirstAsset = AssetDatabase.GUIDToAssetPath(hitsGUIDs[0]);
            _baseMarkerFactorySo = AssetDatabase.LoadAssetAtPath<BaseMarkerFactorySO>(pathToFirstAsset);
        }
    }
}
