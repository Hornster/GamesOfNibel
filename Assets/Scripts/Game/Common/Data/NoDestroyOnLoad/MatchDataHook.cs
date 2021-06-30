using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Enums;
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
        [Tooltip("Prefab for spawner config.")]
        [SerializeField]
        private GameObject _spawnerConfigPrefab;
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

        public void OnMapSelected(MapData mapData)
        {
            MatchDataReference.DeleteData();

            MatchDataReference.AddPlayerConfig();
            SetSpawnersData(mapData);
            MatchDataReference.SceneToLoad = mapData.ScenePath;
        }

        private void SetSpawnersData(MapData mapData)
        {
            foreach (var baseCountData in mapData.BasesCount)
            {
                MatchDataReference.AddSpawnerConfig(CreateSpawnData(baseCountData.Key, baseCountData.Value, mapData.BasesData));
            }
            
        }

        
        private SpawnerGroupConfig CreateSpawnData(Teams spawnerTeam, int spawnersCount, List<BaseData> basesData)
        {
            var newSpawnerConfig = Instantiate(_spawnerConfigPrefab);
            var newSpawnerConfigData = newSpawnerConfig.GetComponent<SpawnerGroupConfig>();
            var basesForTeam = GetBasesDataForTeam(basesData, spawnerTeam);

            newSpawnerConfigData.MyTeam = spawnerTeam;
            newSpawnerConfigData.Quantity = spawnersCount;
            newSpawnerConfigData.BasesData = basesForTeam;

            return newSpawnerConfigData;
        }
        private List<BaseData> GetBasesDataForTeam(List<BaseData> basesData, Teams team)
        {
            var teamBasesData = new List<BaseData>();
            foreach (var baseData in basesData)
            {
                if (baseData.BaseTeam == team)
                {
                    teamBasesData.Add(baseData);
                }
            }

            return teamBasesData;
        }
    }
}
