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

        public BaseSubtypeEnum BaseSubtype
        {
            get => _baseSubtype;
            set => _baseSubtype = value;
        }

        [SerializeField]
        private TeamModule _myTeam;
        [SerializeField]
        private GameplayModesEnum _gameMode;
        [SerializeField]
        private BaseSubtypeEnum _baseSubtype;
    }
}
