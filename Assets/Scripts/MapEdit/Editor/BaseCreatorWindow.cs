using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.MapEdit.Editor.Data;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects;
using Assets.Scripts.MapEdit.Editor.Util;
using Data.Util;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MapEdit.Editor
{
    public class BaseCreatorWindow : EditorWindow
    {
        private BaseMarkerFactorySO _baseMarkerFactorySo;
        private bool _requiredRefsGroupFoldout = false;
        private bool _baseRestorationGroupFoldout = false;

        private BaseTypeEnum _baseType;
        private Teams _baseTeam;
        [Tooltip("Where the base shall be spawned. You can change it later.")]
        private Vector2 _basePosition;
        /// <summary>
        /// Cache for base root object reference. If not found - new object is created in its place.
        /// </summary>
        private GameObject _baseRootObject;
        

        [MenuItem(SGMapEditConstants.WindowsPath + "/Base Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<BaseCreatorWindow>("Base Creator Window");
            window.RetrieveFactorySO();
        }

        public void Awake()
        {
            RetrieveFactorySO();
        }

        private void RetrieveFactorySO()
        {
            var assetSeeker = new AssetSeeker<BaseMarkerFactorySO>();
            _baseMarkerFactorySo = assetSeeker.FindAsset(
                SGMapEditConstants.MapEditBaseMarkersSOsFolderPath, BaseMarkerFactorySO.BaseMarkerFactorySoName);
        }

        private void ChkBasesRootObjectPresence()
        {
            var basesRepositioner = FindObjectOfType<BasesRepositioner>();
            if (basesRepositioner != null)
            {
                _baseRootObject = basesRepositioner.gameObject;
            }
            else
            {
                _baseRootObject = _baseMarkerFactorySo.CreateBaseRoot();
                NotifySceneAboutChanges(_baseRootObject, SGMapEditMessages.AddBaseMarkerRootUndoAction);
            }
        }

        private void NotifySceneAboutChanges(GameObject addedObject, string undoMessage)
        {

            Undo.RecordObject(addedObject, undoMessage);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        private void ResetBasesToBaseRoot()
        {
            var bases = FindObjectsOfType<BaseMarkerData>();

            foreach (var baseObject in bases)
            {
                baseObject.transform.parent = _baseRootObject.transform;
            }
        }

        private void AddBaseMarkerToRoot(GameObject newBaseMarker)
        {
            if (_baseRootObject == null)
            {
                ChkBasesRootObjectPresence();
                ResetBasesToBaseRoot();
            }

            newBaseMarker.transform.parent = _baseRootObject.transform;
        }
        private void OnGUI()
        {
            CreateRequiredReferencesGroup();
            GUILayout.Label("Base settings", EditorStyles.boldLabel);
            _baseType = (BaseTypeEnum) EditorGUILayout.EnumPopup("Base subtype", _baseType);
            _baseTeam = (Teams) EditorGUILayout.EnumPopup("Base team", _baseTeam);
            _basePosition = EditorGUILayout.Vector2Field("Base position", _basePosition);

            if (GUILayout.Button("Create base!"))
            {
                var newBaseMarker = _baseMarkerFactorySo?.CreateBaseMarker(_baseTeam, _baseType, _basePosition);
                AddBaseMarkerToRoot(newBaseMarker);
                BaseMarkersCache.GetInstance().BasesAdded = true;
                NotifySceneAboutChanges(newBaseMarker, SGMapEditMessages.AddBaseMarkerUndoAction);
            }
            CreateBaseRestorationGroup();
        }



        private void CreateBaseRestorationGroup()
        {
            _baseRestorationGroupFoldout = EditorGUILayout.Foldout(_baseRestorationGroupFoldout, "Base Restoration Group");
            if (_baseRestorationGroupFoldout)
            {
                var previousFoldout = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox(SGMapEditMessages.BaseCreatorWindowBaseRootRecheckInfo, MessageType.Info);
                if (GUILayout.Button("Check bases against the root"))
                {
                    ChkBasesRootObjectPresence();
                    ResetBasesToBaseRoot();
                }
            
                EditorGUI.indentLevel = previousFoldout;
            }
        }
        private void CreateRequiredReferencesGroup()
        {
            _requiredRefsGroupFoldout = EditorGUILayout.Foldout(_requiredRefsGroupFoldout, "Required references");
            if (_requiredRefsGroupFoldout)
            {
                var previousFoldout = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox(SGMapEditMessages.BaseCreatorWindowReqPrefsInfo, MessageType.Info);
                _baseMarkerFactorySo = EditorGUILayout.ObjectField("Factory object", _baseMarkerFactorySo, typeof(BaseMarkerFactorySO), false) as BaseMarkerFactorySO;

                EditorGUI.indentLevel = previousFoldout;
            }

        }

    }
}
