using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu.MapSelection
{
    /// <summary>
    /// Main class for the map selection menu.
    /// </summary>
    public class MapSelectionMenuController : MonoBehaviour
    {
        [SerializeField]
        private MapDataSO _debugMapData;
        private void Start()
        {
            var debugMapToJSON = JsonUtility.ToJson(_debugMapData);

            Debug.Log(debugMapToJSON);
        }
    }
}
