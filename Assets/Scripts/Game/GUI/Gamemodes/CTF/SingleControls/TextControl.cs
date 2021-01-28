using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GUI.Gamemodes.CTF.SingleControls
{
    /// <summary>
    /// A GUI text based control that stores a number.
    /// </summary>
    [RequireComponent(typeof(Timer), typeof(TMP_Text))]
    public class TextControl : MonoBehaviour, IGuiTextControl
    {
        private Timer _timer;
        /// <summary>
        /// The control associated with this controller.
        /// </summary>
        private TMP_Text _textControl;
        /// <summary>
        /// For how long the updated message should be seen by the players. 0 and negative numbers
        /// make it be shown FOREVERRR. In seconds.
        /// </summary>
        [SerializeField]
        private float _messageShowingTime = -1.0f;
        /// <summary>
        /// For how long the updated message should be fading when the time comes for it to disappear.
        /// 0 and negative numbers make it disappear instantly. In seconds.
        /// </summary>
        [SerializeField]
        private float _fadingTime = -1.0f;
        /// <summary>
        /// Old value of the alpha channel of the fading text.
        /// </summary>
        private float _oldAlphaValue;
        /// <summary>
        /// When the text needs to be shown, is set to true. Is set to false when the text shall disappear.
        /// </summary>
        private bool _isActive = false;

        private void Awake()
        {
            _textControl = GetComponent<TMP_Text>();
            _timer = GetComponent<Timer>();
            _timer.MaxAwaitTime = _messageShowingTime;

            _oldAlphaValue = _textControl.color.a;
        }
        /// <summary>
        /// Gradually makes the message fade away.
        /// </summary>
        /// <param name="currentFadeTime">Current time of fading.</param>
        private void FadeMessage(float currentFadeTime)
        {
            var currentFadeFactor = 1 - Mathf.Abs(currentFadeTime / _fadingTime);
            if (currentFadeFactor < 0f)
            {
                currentFadeFactor = 0f;
            }

            _textControl.alpha = _oldAlphaValue * currentFadeFactor;
        }
        /// <summary>
        /// Handler for hiding the message. A kind of adapter.
        /// </summary>
        private void HandleHideMessage()
        {
            HideMessage();
        }
        /// <summary>
        /// Hides the message control.
        /// </summary>
        /// <param name="instant">Set to true if the fading sequence shall be omitted.</param>
        public void HideMessage(bool instant = false)
        {
            _timer.ClearTimeoutHandlers();
            if (_fadingTime > 0.0f && instant == false) //If fading is defined, prepare the control for it.
            {
                _timer.RegisterPeriodicHandler(FadeMessage);
                ResetTimer(_fadingTime);
            }
            else
            {
                _textControl.alpha = 0.0f; //Hide the message.
            }
        }
        /// <summary>
        /// Resets, changes max time value and starts the timer instantly after.
        /// </summary>
        /// <param name="newMaxTime"></param>
        private void ResetTimer(float newMaxTime)
        {
            _timer.ResetAndStop();
            _timer.MaxAwaitTime = newMaxTime;
            _timer.StartTimer();
        }
        /// <summary>
        /// Shows the provided message on screen, using assigned control.
        /// </summary>
        /// <param name="newValue"></param>
        public void ShowMessage(string newValue)
        {
            if (_messageShowingTime > 0.0f)
            {
                _timer.ClearPeriodicHandler();
                _timer.RegisterTimeoutHandler(HandleHideMessage);
                ResetTimer(_messageShowingTime);
            }
            _textControl.alpha = _oldAlphaValue;  //Show the message.
            _textControl.text = newValue;
        }
        /// <summary>
        /// Changes the color of the control to provided one.
        /// </summary>
        /// <param name="newColor"></param>
        public void ChangeColor(Color newColor)
        {
            _textControl.color = newColor;
            _oldAlphaValue = newColor.a;
        }
    }
}
