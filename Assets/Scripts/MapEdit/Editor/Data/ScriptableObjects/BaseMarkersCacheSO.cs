using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects
{
    public class BaseMarkersCacheSO : UnityEditor.Editor
    {
        private List<BaseMarkerData> _basesDatas = new List<BaseMarkerData>();
        private bool BasesAdded = false;
        public void FindBases()
        {
            var foundBaseMarkers = FindObjectsOfType<BaseMarkerData>();
            _basesDatas = foundBaseMarkers.ToList();
        }
        private void ChkReferences()
        {
            for(int i = _basesDatas.Count-1; i >= 0; i++)
            {
                if (_basesDatas[i] == null)
                {
                    _basesDatas.RemoveAt(i);
                }
            }
        }
        
    }
}
