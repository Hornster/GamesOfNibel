using System.Collections.Generic;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Localization;
using Assets.Scripts.Game.GameModes.CTF.Observers;
using Assets.Scripts.Game.GUI.Gamemodes.CTF;
using UnityEngine;

namespace Assets.Scripts.Game.GameModes.Managers
{
    /// <summary>
    /// Manages the capture the flag game mode play.
    /// </summary>
    [RequireComponent(typeof(Timer))]
    public class CtfGameModeManager : MonoBehaviour
    {
        /// <summary>
        /// The await time for the players to prepare before the match starts.
        /// </summary>
        [SerializeField]
        private float _startTime = 15.0f;
        /// <summary>
        /// How many flags does given team has to capture in order to win?
        /// </summary>
        [SerializeField]
        private int _victoryPointsRequired;
        /// <summary>
        /// Reference to the gui controllers. Used to show the game state to the players.
        /// </summary>
        [SerializeField]
        private List<CtfGuiController> _guiControllers;
        /// <summary>
        /// Set to true as long as the match is being played. When match ending requirements are met, the match ends and
        /// the flag is set to false/
        /// </summary>
        private bool _isMatchOn;

        private Timer _roundTimer;
        /// <summary>
        /// Stores the score count of the teams.
        /// </summary>
        private Dictionary<Teams, int> _scoreCount = new Dictionary<Teams, int>();
        /// <summary>
        /// Sets the UI controller used to communicate with UI.
        /// </summary>
        /// <param name="ctfUiController"></param>
        public void AddUIController(CtfGuiController ctfUiController)
        {
            _guiControllers.Add(ctfUiController);
        }
        /// <summary>
        /// Starts the match.
        /// </summary>
        public void StartMatch()
        {
            _roundTimer.StartTimer();
            _isMatchOn = true;
        }
        private void Start()
        {
            RegisterFlagCaptureHandler();
            _roundTimer = GetComponent<Timer>();
            _roundTimer.RegisterTimeoutHandler(StartRound);
            _roundTimer.MaxAwaitTime = _startTime;
            _scoreCount.Add(Teams.Lily, 0);
            _scoreCount.Add(Teams.Lotus, 0);
        }
        /// <summary>
        /// Victory has been achieved - report it.
        /// </summary>
        /// <param name="whichTeam">What team has recently stored a point?</param>
        private void ReportVictory(Teams whichTeam)
        {
            _isMatchOn = false;
            Debug.Log($"Team {whichTeam} is victorious!!");
            var locale = LocalizationManager.GetInstance();
            var victoryMsg = locale.CtfLocale.GetVictoryQuote(whichTeam);

            foreach (var guiController in _guiControllers)
            {
                guiController.PrintMessage(whichTeam, victoryMsg);
            }
        }
        /// <summary>
        /// Ends the match, returns to menu.
        /// </summary>
        private void EndMatch()
        {
            //TODO
        }
        /// <summary>
        /// Checks the victory condition for N captures.
        /// </summary>
        /// <param name="whichTeam">What team has recently stored a point?</param>
        private void ChkVictoryCondition(Teams whichTeam)
        {
            if (_scoreCount.TryGetValue(whichTeam, out var currentPoints))
            {
                if (currentPoints >= _victoryPointsRequired)
                {
                    ReportVictory(whichTeam);
                }
            }
            else
            {
                Debug.LogError($"How on Nibel did you get here?! There's no such team as {whichTeam}!!!");
            }
        }
        /// <summary>
        /// Called when a team scored a point.
        /// </summary>
        private void TeamScoredPoint(Teams whichTeam, int pointsAmount)
        {
            if (_isMatchOn == false)
            {
                return; //Do not check for points if the match has ended already. No point in that.
            }

            if (_scoreCount.TryGetValue(whichTeam, out var currentPoints))
            {
                currentPoints += pointsAmount;
                _scoreCount[whichTeam] = currentPoints;

                foreach (var guiController in _guiControllers)
                {
                    guiController.UpdatePoints(whichTeam, currentPoints);
                }

                ChkVictoryCondition(whichTeam);
            }
            else
            {
                Debug.LogError($"How on Nibel did you get here?! There's no such team as {whichTeam}!!!");
            }
        }

        private void RegisterFlagCaptureHandler()
        {
            var flagCapturers = GetComponentsInChildren<IFlagCapturedObserver>();
            foreach (var flagCapturer in flagCapturers)
            {
                flagCapturer.RegisterObserver(TeamScoredPoint);
            }
        }
        /// <summary>
        /// Begins the round after the initial countdown.
        /// </summary>
        private void StartRound()
        {
            _roundTimer.ResetAndStop();

        }
    }
}
