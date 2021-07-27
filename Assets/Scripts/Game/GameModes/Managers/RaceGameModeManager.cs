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
        private class PlayerRaceData : IReset
        {
            public bool PlayerFinishedRace { get; set; }
            public RaceGUIController PlayerGUIController { get; set; }
            public float PlayerTime { get; set; }

            public void Reset()
            {
                PlayerTime = 0f;
                PlayerFinishedRace = false;
            }
        }

        private Dictionary<int, PlayerRaceData> _playerStates = new Dictionary<int, PlayerRaceData>();
        private List<IReset> _objectsToReset = new List<IReset>();

        /// <summary>
        /// Reference to the gui controllers. Used to show the game state to the players.
        /// </summary>
        [SerializeField]
        private List<RaceGUIController> _guiControllers = new List<RaceGUIController>();

        private void Start()
        {
            _roundTimer = GetComponent<Timer>();
            _roundTimer.MaxAwaitTime = _startTime;
            _roundTimer.RegisterTimeoutHandler(StartRound);
        }

        private void Update()
        {
            var time = GetTimeToShow(); 
            foreach (var playerData in _playerStates)
            {
                var playerRaceData = playerData.Value;
                if (playerRaceData.PlayerFinishedRace == false)
                {
                    playerRaceData.PlayerGUIController.UpdateCounter(ref time, !_isRoundOn);
                }
            }
        }

        private void ChkRoundEndCondition()
        {
            foreach (var playerState in _playerStates.Values)
            {
                if (playerState.PlayerFinishedRace == false)
                {
                    return;
                }
            }

        }
        /// <summary>
        /// Gets the format of the time to be shown, basing on current round status (if the round is on or countdown to start is being performed).
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetTimeToShow()
        {
            float currentTime;

            if (_isRoundOn)
            {
                currentTime = _roundTimer.CurrentTime;
            }
            else
            {
                currentTime = -(_startTime - _roundTimer.CurrentTime);
            }

            return TimeSpan.FromSeconds(currentTime);
        }
        private void PlayerFinishedRace(int playerID)
        {
            if(_isRoundOn == false)
            {
                return; //Don't even bother if the round hasn't started yet.
            }

            if (_playerStates.TryGetValue(playerID, out var raceData))
            {
                if (raceData.PlayerFinishedRace)
                {
                    return;//If the player has already finished - we don't need to change anything.
                }

                var playerTimeSpan = TimeSpan.FromSeconds(_roundTimer.CurrentTime);
                raceData.PlayerFinishedRace = true;
                raceData.PlayerTime = _roundTimer.CurrentTime;
                raceData.PlayerGUIController.UpdateCounter(ref playerTimeSpan, !_isRoundOn);
            }
        }
        /// <summary>
        /// Starts current round after initial countdown.
        /// </summary>
        private void StartRound()
        {
            foreach (var uiController in _guiControllers)
            {
                uiController.PrintMessage(Teams.Multi, "Round begins!");
            }
            _roundTimer.Reset();
            _isRoundOn = true;
            _roundTimer.ClearTimeoutHandlers();
        }

        public void AddPlayerGUI(RaceGUIController raceGUIController)
        {
            if (_playerStates.ContainsKey(raceGUIController.OwningPlayerID))
            {
                throw new Exception(ErrorMessages.DuplicatePlayerIDFound);
            }

            var playerData = new PlayerRaceData
            {
                PlayerFinishedRace = false,
                PlayerGUIController = raceGUIController,
            };

            _guiControllers.Add(raceGUIController);
            _playerStates.Add(raceGUIController.OwningPlayerID, playerData);
            AddResetComponent(playerData);
        }
        public void AddResetComponent(IReset componentToReset)
        {
            _objectsToReset.Add(componentToReset);
        }
        /// <summary>
        /// Resets the state of the round, allowing for racing again without the need of reloading the map.
        /// </summary>
        public void ResetRound()
        {
            foreach(var objectToReset in _objectsToReset)
            {
                objectToReset.Reset();
            }
            _roundTimer.Reset();
            _roundTimer.RegisterTimeoutHandler(StartRound);
            _isRoundOn = false;
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
        /// <summary>
        /// Begins the match itself, starting from beginning countdown.
        /// </summary>
        public override void StartMatch()
        {
            foreach (var uiController in _guiControllers)
            {
                uiController.PrintMessage(Teams.Multi, $"Round begins in {_startTime} seconds!");
            }
            _roundTimer.StartTimer();
            SceneManager.sceneLoaded -= OnSceneLoaded;  //The match began - unregister the event handler.
        }


        public void RegisterPlayerFinishedRaceHandler(RaceFinishBaseController raceFinishBase)
        {
            raceFinishBase.RegisterPlayerArrivedEvent(PlayerFinishedRace);
        }
    }
}
