using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.ScriptableObjects.ScenePassData
{
    /// <summary>
    /// Holds path to the loading scene - temporary scene that loads all dynamic objects, like spawners and
    /// players, for map scene.
    /// </summary>
    [CreateAssetMenu(fileName = LoadingScenePathSOName, 
        menuName = SGConstants.SGSOMenuPath + SGConstants.SGSOSceneChangingRelativePath + LoadingScenePathSOName, order=2)]
    public class LoadingScenePathSO : ScriptableObject
    {
        public const string LoadingScenePathSOName = "LoadingScenePathSO";
        [Tooltip("Path to the loading scene which initializes data before the map scene is loaded.")]
        [SerializeField]
        private string _loadingScenePath;
        /// <summary>
        /// Path to the loading scene that loads dynamic objects like players and spawns.
        /// </summary>
        public string LoadingScenePath => _loadingScenePath;
    }
}
