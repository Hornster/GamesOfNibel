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
    [CreateAssetMenu(fileName = BaseMarkersCacheSOName, menuName = SGMapEditPaths.MapEditMenuPath + BaseMarkersCacheSOName)]
    public class BaseMarkersCacheSO : ScriptableObject
    {
        public const string BaseMarkersCacheSOName = "BaseMarkersCacheSO";
        private List<GameObject> _knownBases = new List<GameObject>();

        public void AddBase(GameObject baseMainGameObject)
        {
            //TODO: This won't work. GameObjects cannot be stored in SO. Try out FindObjectByType instead
            _knownBases.Add(baseMainGameObject);
            EditorUtility.SetDirty(this);
        }
        /// <summary>
        /// Checks if base markers are still present. If given marker is not present
        /// removes it from the list.
        /// </summary>
        public void ChkBases()
        {
            for (int i = _knownBases.Count - 1; i >= 0; i--)
            {
                if (_knownBases[i] == null)
                {
                    _knownBases.RemoveAt(i);
                }
            }
            EditorUtility.SetDirty(this);
        }
        /// <summary>
        /// Gets BaseMarkerData components from all known bases. Checks what bases are
        /// still present beforehand.
        /// </summary>
        /// <returns></returns>
        public List<BaseMarkerData> GetBasesData()
        {
            ChkBases();

            var basesData = new List<BaseMarkerData>();
            foreach (var knownBase in _knownBases)
            {
                basesData.Add(knownBase.GetComponent<BaseMarkerData>());
            }

            return basesData;
        }
    }
}
