using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.GUI.Gamemodes.Race
{
    public class RaceGUIController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _timeCounter;

        public void UpdateCounter(TimeSpan time)
        {
            _timeCounter.text = time.ToString(@"hh\:mm\:ss.fff");
        }
    }
}