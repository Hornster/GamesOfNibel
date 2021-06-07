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
        private static int _lastUsedID = 0;
        /// <summary>
        /// Unique ID of the base
        /// </summary>
        public int ID { get; private set; }//TODO: maybe do it in factory. Factory is in SO, with this value, so it's remembered. Make it reset upon reaching 300 000 000 or something
        [SerializeField]
        private TeamModule _myTeam;
        [SerializeField]
        private GameplayModesEnum _gameMode;
        [SerializeField]
        private BaseSubtypeEnum _baseSubtype;

        private void Start()
        {
            _lastUsedID += 1;
            ID = _lastUsedID;
        }
    }
}
