using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.GUI.Menu.InGameMenu;
using UnityEngine;
using UnityEngine.Events;
using Input = UnityEngine.Input;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts.Game.Player
{
    public class InputReader : MonoBehaviour
    {
        //Events
        private static UnityAction _jumpHandler;
        private static UnityAction _prematureJumpEndHandler;
        private static UnityAction<GlideStages> _glideHandler;
        private static UnityAction _jumpingOffPlatformsStartHandler;
        private static UnityAction _jumpingOffPlatformsEndHandler;
        private static UnityAction _helpToggleHandler;
        private static UnityAction _gameLeaveHandler;
        /// <summary>
        /// Handler for special activities that differ among the gamemodes, like flag drop.
        /// </summary>
        private static UnityAction _specialActivityGameModeHandler;

        //Private variables
        private ButtonConfig _buttonConfig;
        /// <summary>
        /// Set to true when player pressed the jump button and is holding down button
        /// to jump off the platform.
        /// </summary>
        private bool _isJumpOffPlatformActive;
        /// <summary>
        /// If set to true, will keep checking input from the player. Turning this to false disables checks.
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        /// Stores the raw values of the input read from base axes.
        /// </summary>
        public static Vector2 InputRaw { get; private set; }

        private void Start()
        {
            _buttonConfig = ButtonConfig.GetInstance();
            InGameMenuToggler.RegisterOnMenuToggledHandler(MenuToggled);
        }

        private void Update()
        {
            if (_isEnabled == false)
            {
                return;
            }

            ChkAxes();
            ChkControls();
            ChkSpecialCases();
        }
        /// <summary>
        /// Checks the state of the axes.
        /// </summary>
        private void ChkAxes()
        {
            InputRaw = new Vector2(Input.GetAxisRaw(ButtonConfig.HorizontalAxisName), Input.GetAxisRaw(ButtonConfig.VerticalAxisName));
        }
        /// <summary>
        /// Checks for special cases concerning controls.
        /// </summary>
        private void ChkSpecialCases()
        {
            if (_isJumpOffPlatformActive)
            {
                if (Input.GetKeyUp(_buttonConfig.JumpButton) || InputRaw.Y >= 0.0f)
                {
                    _isJumpOffPlatformActive = false;
                    _jumpingOffPlatformsEndHandler?.Invoke();
                }
            }
        }
        /// <summary>
        /// Checks whether any special buttons have been pressed. If yes - calls proper handlers, if these are not null.
        /// </summary>
        private void ChkControls()
        {
            if (Input.GetKeyDown(_buttonConfig.JumpButton))
            {
                if (InputRaw.Y < 0.0f)
                {
                    //The player wants to drop from some droppable platform.
                    _jumpingOffPlatformsStartHandler?.Invoke();
                    _isJumpOffPlatformActive = true;
                }
                else
                {
                    _jumpHandler?.Invoke();
                }
            }
            else if (Input.GetKeyUp(_buttonConfig.JumpButton))
            {
                _prematureJumpEndHandler?.Invoke();
            }

            if (Input.GetKeyDown(_buttonConfig.GlideButton))
            {
                _glideHandler?.Invoke(GlideStages.GlideBegin);
            }
            else if (Input.GetKey(_buttonConfig.GlideButton))
            {
                _glideHandler?.Invoke(GlideStages.GlideKeep);
            }
            else if (Input.GetKeyUp(_buttonConfig.GlideButton))
            {
                _glideHandler?.Invoke(GlideStages.GlideStop);
            }

            if (Input.GetKeyDown(_buttonConfig.HelpButton))
            {
                _helpToggleHandler?.Invoke();
            }
            if (Input.GetKeyDown(_buttonConfig.LeaveButton))
            {
                _gameLeaveHandler?.Invoke();
            }

            if (Input.GetKeyDown(_buttonConfig.ModeSpecialButton))
            {
                _specialActivityGameModeHandler?.Invoke();
            }
        }
        /// <summary>
        /// Used to toggle the input reader.
        /// </summary>
        /// <param name="inGameMenuToggled"> Is the in-game menu currently active?</param>
        private void MenuToggled(bool inGameMenuToggled)
        {
            //Input reader is all about player movement during match. We do not want to use it when
            //the menu is active.
            _isEnabled = !inGameMenuToggled;
        }

        #region EventRegistering

        /// <summary>
        /// Registers jump handler.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterJumpHandler(UnityAction handler)
        {
            _jumpHandler += handler;
        }
        /// <summary>
        /// Allows for registration of prematurely jump ending by releasing the jump key too fast.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterPrematureJumpEndHandler(UnityAction handler)
        {
            _prematureJumpEndHandler += handler;
        }
        /// <summary>
        /// Registers glide handler.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterGlideHandler(UnityAction<GlideStages> handler)
        {
            _glideHandler += handler;
        }
        /// <summary>
        /// Registers handler from dropping down from one-way platform.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterJumpOffPlatformStartHandler(UnityAction handler)
        {
            _jumpingOffPlatformsStartHandler += handler;
        }
        /// <summary>
        /// Registers handler from dropping down from one-way platform.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterJumpOffPlatformEndHandler(UnityAction handler)
        {
            _jumpingOffPlatformsEndHandler += handler;
        }
        /// <summary>
        /// Registers glide handler.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterHelpToggleHandler(UnityAction handler)
        {
            _helpToggleHandler += handler;
        }
        /// <summary>
        /// Registers game ending handler.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterGameEnd(UnityAction handler)
        {
            _gameLeaveHandler += handler;
        }
        /// <summary>
        /// Registers the handler for special activity connected with currently selected game mode.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterSpecialModeActivity(UnityAction handler)
        {
            _specialActivityGameModeHandler += handler;
        }

        #endregion EventRegistering

        private void OnDestroy()
        {
            _jumpHandler = null;
            _prematureJumpEndHandler = null;
            _glideHandler = null;
            _jumpingOffPlatformsStartHandler = null;
            _jumpingOffPlatformsEndHandler = null;
            _helpToggleHandler = null;
            _gameLeaveHandler = null;
            _specialActivityGameModeHandler = null;
        }
    }
}
