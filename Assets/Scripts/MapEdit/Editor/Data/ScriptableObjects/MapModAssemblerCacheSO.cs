using Assets.Scripts.Game.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = MapModAssemblerCacheSoName, menuName = SGMapEditConstants.MapEditMenuPath + MapModAssemblerCacheSoName)]
    public class MapModAssemblerCacheSO : ScriptableObject
    {
        public const string MapModAssemblerCacheSoName = "MapModAssemblerCacheSO";

        [Tooltip("The last input into map mod assembler scene.")]
        [SerializeField]
        private SceneAsset _lastSetScene;
        [Tooltip("The last input into map mod assembler map data scriptable object.")]
        [SerializeField]
        private MapDataSO _lastMapDataSO;

        /// <summary>
        /// Last used scene asset.
        /// </summary>
        public SceneAsset LastSetScene
        {
            get => _lastSetScene;
            set => _lastSetScene = value;
        }
        /// <summary>
        /// Last used map data so.
        /// </summary>
        public MapDataSO LastMapDataSO
        {
            get => _lastMapDataSO;
            set => _lastMapDataSO = value;
        }
    
    }
}
