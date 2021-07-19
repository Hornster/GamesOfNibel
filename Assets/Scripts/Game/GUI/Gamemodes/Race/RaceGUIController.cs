using System;
using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            _informationMessage = _informationMsgContainer.Interface;

            _informationMessage.HideMessage(true);
        }

        public void UpdateCounter(TimeSpan time)
        {
            _timeCounter.text = time.ToString(@"hh\:mm\:ss.fff");
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
    }
}