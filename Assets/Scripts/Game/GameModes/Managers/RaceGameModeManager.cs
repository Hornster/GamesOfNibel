using Assets.Scripts.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data.Constants;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.GameModes.Race;
using Assets.Scripts.Game.GUI.Gamemodes.Race;
using Assets.Scripts.Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.GameModes.Managers
{
    
    [RequireComponent(typeof(Timer))]
    public class RaceGameModeManager : GameModeManager
    {
        private class PlayerRefs
        {
            public bool PlayerFinishedRace { get; set; }
            public RaceGUIController PlayerGUIController { get; set; }
        }

        private  Dictionary<int, PlayerRefs> _players = new Dictionary<int, PlayerRefs>();

        /// <summary>
        /// Reference to the gui controllers. Used to show the game state to the players.
        /// </summary>
        [SerializeField]
        private List<RaceGUIController> _guiControllers = new List<RaceGUIController>();

        private void Start()
        {
            _roundTimer.MaxAwaitTime = _startTime;
            _roundTimer.RegisterTimeoutHandler(StartRound);
        }

        private void Update()
        {
            var time = TimeSpan.FromSeconds(_roundTimer.CurrentTime);
            foreach (var guiController in _guiControllers)
            {
                guiController.UpdateCounter(ref time);
            }
        }

        private void StartRound()
        {
            foreach (var uiController in _guiControllers)
            {
                uiController.PrintMessage(Teams.Multi, "Round begins!");
            }
            _roundTimer.Reset();
            _isMatchOn = true;
        }

        public void AddPlayer(GameObject player)
        {
            var playerRaceUI = player.GetComponentInChildren<RaceGUIController>();
            var playerController = player.GetComponentInChildren<PlayerController>();

            if (_players.ContainsKey(playerController.PlayerID))
            {
                throw new Exception(ErrorMessages.DuplicatePlayerIDFound);
            }

            _players.Add(playerController.PlayerID, new PlayerRefs
            {
                PlayerFinishedRace = false,
                PlayerGUIController = playerRaceUI,
            });
        }

        public void ResetGame()
        {
            foreach (var player in _players)
            {
                player.Value.PlayerFinishedRace = false;
            }
        }
        //TODO Make player controller have an ID assigned through factory.  DONE
        //TODO Factory would receive the id from data object    DONE
        //TODO This class would  have refs to all players' GUIs and boolean indicating if they finished the run. DONE
        //TODO Or players would have special component for racing, idk. NOPE
        //TODO The refs and boolean would be in dict, ID would be the key.  DONE
        //TODO upon running into the finish line the ID would be sent
        //TODO this class would pass the victory info and stop the timers.
        //TODO In Update, whoever has not finished the race yet would have their UI updated every frame.
        //TODO You could use timer with TimeOffset (or something like that) structure from .Net. It could be initialized with
        //TODO seconds, then read hours, minutes and seconds part for easy formatting.
        public override void StartMatch()
        {
            foreach (var uiController in _guiControllers)
            {
                uiController.PrintMessage(Teams.Multi, $"Round begins in {_startTime} seconds!");
            }
            _roundTimer.StartTimer();
            SceneManager.sceneLoaded -= OnSceneLoaded;  //The match began - unregister the event handler.
        }
    }
}
