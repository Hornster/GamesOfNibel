using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using UnityEditor;

namespace Assets.Scripts.Editor.CustomEditors
{
    [CustomEditor(typeof(GlobalGravityManager))]
    public class GlobalGravityManagerEditor : UnityEditor.Editor
    {
        private string _infoText = SGMapEditMessages.GlobalGravityManagerInfo;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(SGMapEditMessages.GlobalGravityManagerInfo, MessageType.Info, true);

            base.OnInspectorGUI();
        }
    }
}
