using Assets.Scripts.Game.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.Maps
{
    /// <summary>
    /// Class directly used to serialize and deserialize base data.
    /// </summary>
    public class BaseDataGOAdapter : MonoBehaviour
    {
        /// <summary>
        /// Unique ID of the base.
        /// </summary>
        [SerializeField]
        private int _id;
        [SerializeField]
        private TeamModule _baseTeam;
        [SerializeField]
        private GameplayModesEnum _gameMode;
        [SerializeField]
        private BaseTypeEnum _baseType;

        public int ID { get => _id; set => _id = value; }
        public TeamModule BaseTeam { get => _baseTeam; set => _baseTeam = value; }
        public GameplayModesEnum GameMode { get => _gameMode; set => _gameMode = value; }
        public BaseTypeEnum BaseType { get => _baseType; set => _baseType = value; }
    }
}
