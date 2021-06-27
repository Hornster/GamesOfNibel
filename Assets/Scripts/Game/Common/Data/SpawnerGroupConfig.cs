using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Enums;
using Boo.Lang;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data
{
    /// <summary>
    /// Stores config for all spawners for given team.
    /// </summary>
    public class SpawnerGroupConfig : MonoBehaviour
    {
        [Tooltip("Team this spawner belongs to.")]
        [SerializeField]
        private Teams _myTeam;
        [Tooltip("How many spawns of this type should be created for the map.")]
        [SerializeField]
        private int _quantity;

        private void Awake()
        {
            //In case that someone wanted to test bases loading wihtout loading the map,
            //all they have to do is add the BaseDataGOAdapters as components of children
            //of the object with this script. These will be read here and added to base data.
            var staticBaseData = GetComponentsInChildren<BaseDataGOAdapter>();

            foreach(var baseData in staticBaseData)
            {
                var serializableBaseData = new BaseData();
                serializableBaseData.BaseTeam = baseData.BaseTeam.MyTeam;
                serializableBaseData.BaseType = baseData.BaseType;
                serializableBaseData.ID = baseData.ID;
                serializableBaseData.GameMode = baseData.GameMode;

                BasesData.Add(serializableBaseData);
            }
        }

        /// <summary>
        /// What team does the spawn group belong to.
        /// </summary>
        public Teams MyTeam { get => _myTeam; set => _myTeam = value; }
        /// <summary>
        /// OBSOLETE How many spawns of this team there should be.
        /// </summary>
        public int Quantity { get => _quantity; set => _quantity=value; }

        public List<BaseData> BasesData { get; set; } = new List<BaseData>();
        /// <summary>
        /// Destroys the config's gameobject.
        /// </summary>
        public void DestroyConfig()
        {
            Destroy(this.gameObject);
        }
    }
}
