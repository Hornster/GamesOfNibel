using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Manages reference to the Matchdata object, making sure only one is present at any given time.
    /// When it is not present - spawns it during loading.
    /// </summary>
    public class MatchDataHook : MonoBehaviour
    {
        [Tooltip("The prefab of the match data object.")]
        [SerializeField]
        private GameObject _matchDataPrefab;
        
        /// <summary>
        /// Static reference to the match data. Used to control the presence of the singleton.
        /// MatchData is marked as DontDestroyOnLoad, so it requires higher control level.
        /// </summary>
        public static MatchData MatchDataReference { get; private set; }

        private void Start()
        {
            if (MatchDataReference == null)
            {
                var matchDataObj = Instantiate(_matchDataPrefab);
                var matchDataScript = matchDataObj.GetComponentInChildren<MatchData>();
                MatchDataReference = matchDataScript;
                //TODO add saving the ref to the indestructible object.
                //TODO connect the object with SkillsState through regular ref if necessary
            }
        }
    }
}
