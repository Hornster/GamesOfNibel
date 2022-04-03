using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.GUI.Gamemodes.CTF;
using Assets.Scripts.Game.GUI.Gamemodes.CTF.SingleControls;
using Assets.Scripts.Game.InspectorSerialization.Interfaces;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.GUI.Gamemodes.Race
{
    public class RaceGUIController : MonoBehaviour, IGUIInterface
    {
        /// <summary>
        /// The message showed to all players.
        /// </summary>
        [SerializeField]
        private ITextControlContainer _informationMsgContainer;
        private IGuiTextControl _informationMessage;

        [SerializeField]
        private TMP_Text _timeCounter;
        public int OwningPlayerID { get; set; }

        private void Start()
        {
            _informationMessage = _informationMsgContainer.Interface;

            _informationMessage.HideMessage(true);
        }
        /// <summary>
        /// Updates the value shown by the counter.
        /// </summary>
        /// <param name="time">Time to show.</param>
        /// <param name="isNegative">Setting this to TRUE adds a minus sign at the beginning.</param>
        public void UpdateCounter(ref TimeSpan time, bool isNegative)
        {
            string textToShow = string.Empty;
            if (isNegative)
            {
                textToShow += "-";
            }
            textToShow += time.ToString(@"hh\:mm\:ss\.fff");

            _timeCounter.text = textToShow;
        }


        public void PrintMessage(Teams whichTeam, string message)
        {
            SwitchMsgColor(_informationMessage, whichTeam);
            _informationMessage.ShowMessage(message);
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

        public void ShowUnstuck(KeyStateEnum keyState)
        {
            throw new NotImplementedException();
        }
    }
}