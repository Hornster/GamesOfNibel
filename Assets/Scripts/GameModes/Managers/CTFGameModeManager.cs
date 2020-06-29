using Assets.Scripts.Common;
using Assets.Scripts.Common.Enums;
using System.Collections.Generic;
using Assets.Scripts.GameModes.CTF.Observers;
using UnityEngine;

namespace Assets.Scripts.GameModes.Managers
{
    /// <summary>
    /// Manages the capture the flag game mode play.
    /// </summary>
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

        private Timer _roundTimer;
        /// <summary>
        /// Stores the score count of the teams.
        /// </summary>
        private Dictionary<Teams, int> _scoreCount = new Dictionary<Teams, int>();

        private void Start()
        {
            RegisterFlagCaptureHandler();
            _roundTimer = new Timer(_startTime, StartRound);
            _roundTimer.Start();
            _scoreCount.Add(Teams.Lily, 0);
            _scoreCount.Add(Teams.Lotus, 0);
        }

        private void FixedUpdate()
        {
            _roundTimer.Update();
        }
        /// <summary>
        /// Victory has been achieved - report it.
        /// </summary>
        /// <param name="whatTeam">What team has recently stored a point?</param>
        private void ReportVictory(Teams whatTeam)
        {
            Debug.Log($"Team {whatTeam} is victorious!!");
        }
        /// <summary>
        /// Checks the victory condition for N captures.
        /// </summary>
        /// <param name="whatTeam">What team has recently stored a point?</param>
        private void ChkVictoryCondition(Teams whatTeam)
        {
            if (_scoreCount.TryGetValue(whatTeam, out var currentPoints))
            {
                if (currentPoints >= _victoryPointsRequired)
                {
                    ReportVictory(whatTeam);
                }
            }
            else
            {
                Debug.LogError($"How on Nibel did you get here?! There's no such team as {whatTeam}!!!");
            }
        }
        /// <summary>
        /// Called when a team scored a point.
        /// </summary>
        private void TeamScoredPoint(Teams whichTeam, int pointsAmount)
        {
            if (_scoreCount.TryGetValue(whichTeam, out var currentPoints))
            {
                _scoreCount[whichTeam] += pointsAmount;
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
