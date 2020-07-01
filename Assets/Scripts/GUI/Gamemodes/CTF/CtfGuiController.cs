using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GUI.Gamemodes.CTF
{
    /// <summary>
    /// Controller for CTF mode GUI elements.
    /// </summary>
    public class CtfGuiController : MonoBehaviour, ICtfGuiInterface
    {
        private Timer _informationTimer;
        /// <summary>
        /// The message showed to all players.
        /// </summary>
        [SerializeField]
        private TMP_Text _informationMessage;
        /// <summary>
        /// Score for the lily team.
        /// </summary>
        [SerializeField] 
        private TMP_Text _teamLilyScore;
        /// <summary>
        /// Score for the lotus team.
        /// </summary>
        [SerializeField] 
        private TMP_Text _teamLotusScore;

        [SerializeField] private float infoMsgShowingTime;
        [SerializeField] private float infoMsgFadingTime;
        /// <summary>
        /// Changes the color of provided text.
        /// </summary>
        /// <param name="whichMessage">Which control to switch color.</param>
        /// <param name="whichTeam">To what color should the control switch its color.</param>
        private void SwitchMsgColor(TMP_Text whichMessage, Teams whichTeam)
        {
            var whatColor = TeamColors.GetTeamColor(whichTeam);
            whichMessage.color = whatColor;
        }

        private void Start()
        {
            _informationTimer = new Timer(infoMsgShowingTime, HideMessage);
        }

        private void FixedUpdate()
        {
            _informationTimer.Update();
        }

        private void HideMessage()
        {
            //_informationMessage.
        }
        /// <summary>
        /// Pops up the provided message.
        /// </summary>
        /// <param name="whichTeam">What color should the message have.</param>
        /// <param name="message">The message itself.</param>
        public void PrintMessage(Teams whichTeam, string message)
        {
            SwitchMsgColor(_informationMessage, whichTeam);
            _informationTimer.Reset();
            _informationTimer.Start();
        }

        public void UpdatePoints(Teams whichTeam, int value)
        {
            throw new NotImplementedException();
        }
    }
}
//TODO - Every message control should have its own controller. It should know for how long
//TODO - the message shall be shown and in what color.