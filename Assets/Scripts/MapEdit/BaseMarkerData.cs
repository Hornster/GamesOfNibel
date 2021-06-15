using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.MapEdit
{
    public class BaseMarkerData : MonoBehaviour
    {
        /// <summary>
        /// Unique ID of the base
        /// </summary>
        public int ID { get; set; }

        public GameplayModesEnum GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }

        public BaseTypeEnum BaseType
        {
            get => _baseType;
            set => _baseType = value;
        }

        public Teams BaseTeam => _myTeam.MyTeam;

        [SerializeField]
        private TeamModule _myTeam;
        [SerializeField]
        private GameplayModesEnum _gameMode;
        [SerializeField]
        private BaseTypeEnum _baseType;
    }
}
