using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Editor.Constants;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Player.Graphics;
using UnityEditor;

namespace Assets.Scripts.Editor.CustomEditors
{
    [CustomEditor(typeof(PlayerNameController))]
    public class PlayerNameControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //EditorGUILayout.HelpBox(EditorSGConstants.PlayerNameControllerEditorInfo, MessageType.Info);

            base.OnInspectorGUI();
        }
    }
}
