using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.MapEdit.Editor.Data;
using Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects;
using Assets.Scripts.MapEdit.Editor.Util;
using Data.Util;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor
{
    public class BaseCreatorWindow : EditorWindow
    {
        private BaseMarkerFactorySO _baseMarkerFactorySo;
        private bool _requiredRefsGroupFoldout = false;

        private GameplayModesEnum _gameMode;
        private BaseSubtypeEnum _baseSubtype;
        private Teams _baseTeam;
        [Tooltip("Where the base shall be spawned. You can change it later.")]
        private Vector2 _basePosition;
        

        [MenuItem(SGMapEditPaths.WindowsPath + "/Base Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<BaseCreatorWindow>("Base Creator Window");
            var assetSeeker = new AssetSeeker<BaseMarkerFactorySO>();
            window._baseMarkerFactorySo = assetSeeker.FindBaseMarkerFactorySo(
                SGMapEditPaths.MapEditScriptableObjectsPath, BaseMarkerFactorySO.BaseMarkerFactorySoName);
        }

        private void OnGUI()
        {
            CreateRequiredReferencesGroup();
            GUILayout.Label("Base settings", EditorStyles.boldLabel);
            _gameMode = (GameplayModesEnum)EditorGUILayout.EnumPopup("Game mode", _gameMode);
            _baseSubtype = (BaseSubtypeEnum) EditorGUILayout.EnumPopup("Base subtype", _baseSubtype);
            _baseTeam = (Teams) EditorGUILayout.EnumPopup("Base team", _baseTeam);
            _basePosition = EditorGUILayout.Vector2Field("Base position", _basePosition);

            if (GUILayout.Button("Create base!"))
            {
                _baseMarkerFactorySo?.CreateBaseMarker(_baseTeam, _gameMode, _baseSubtype, _basePosition);
            }
        }

        private void CreateRequiredReferencesGroup()
        {
            _requiredRefsGroupFoldout = EditorGUILayout.Foldout(_requiredRefsGroupFoldout, "Required references");
            if (_requiredRefsGroupFoldout)
            {
                var previousFoldout = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox("None of these should be null/empty.", MessageType.Info);
                _baseMarkerFactorySo = EditorGUILayout.ObjectField("Factory object", _baseMarkerFactorySo, typeof(BaseMarkerFactorySO), false) as BaseMarkerFactorySO;

                EditorGUI.indentLevel = previousFoldout;
            }

        }

    }
}
