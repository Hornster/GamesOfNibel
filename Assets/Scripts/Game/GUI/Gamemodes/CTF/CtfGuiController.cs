using System;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.GUI.Gamemodes.CTF.SingleControls;
using Assets.Scripts.Game.InspectorSerialization.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Game.GUI.Gamemodes.CTF
{
    /// <summary>
    /// Controller for CTF mode GUI elements.
    /// </summary>
    public class CtfGuiController : MonoBehaviour, ICtfGuiInterface
    {
        /// <summary>
        /// The message showed to all players.
        /// </summary>
        [SerializeField]
        private ITextControlContainer _informationMsgContainer;
        private IGuiTextControl _informationMessage;

        /// <summary>
        /// Score for the lily team.
        /// </summary>
        [SerializeField]
        private ITextControlContainer _teamLilyScoreContainer;
        private IGuiTextControl _teamLilyScore;
        /// <summary>
        /// Score for the lotus team.
        /// </summary>
        [SerializeField]
        private ITextControlContainer _teamLotusScoreContainer;
        private IGuiTextControl _teamLotusScore;

        private void Start()
        {
            _informationMessage = _informationMsgContainer.Interface;
            _teamLilyScore = _teamLilyScoreContainer.Interface;
            _teamLotusScore = _teamLotusScoreContainer.Interface;

            _informationMessage.HideMessage(true);
            _teamLilyScore.ChangeColor(TeamColorsSO.GetTeamColor(Teams.Lily));
            _teamLotusScore.ChangeColor(TeamColorsSO.GetTeamColor(Teams.Lotus));
            _teamLilyScore.ShowMessage(0.ToString());//Reset the flag count.
            _teamLotusScore.ShowMessage(0.ToString());//Reset the flag count.
        }
        /// <summary>
        /// Changes the color of provided text.
        /// </summary>
        /// <param name="whichMessage">Which control to switch color.</param>
        /// <param name="whichTeam">To what color should the control switch its color.</param>
        private void SwitchMsgColor(IGuiTextControl whichMessage, Teams whichTeam)
        {
            var newColor = TeamColorsSO.GetTeamColor(whichTeam);
            whichMessage.ChangeColor(newColor);
        }
        
        /// <summary>
        /// Pops up the provided message.
        /// </summary>
        /// <param name="whichTeam">What color should the message have.</param>
        /// <param name="message">The message itself.</param>
        public void PrintMessage(Teams whichTeam, string message)
        {
            SwitchMsgColor(_informationMessage, whichTeam);
            _informationMessage.ShowMessage(message);
        }
        /// <summary>
        /// Updates the points of given team.
        /// </summary>
        /// <param name="whichTeam"></param>
        /// <param name="value"></param>
        public void UpdatePoints(Teams whichTeam, int value)
        {
            switch(whichTeam)
            {
                case Teams.Lotus:
                    _teamLotusScore.ShowMessage(value.ToString());
                    break;
                case Teams.Lily:
                    _teamLilyScore.ShowMessage(value.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(whichTeam), whichTeam, "Provided team cannot have their points increased in CTF mode!");
            }
        }
    }
}