using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts.Player
{
    public class InputReader : MonoBehaviour
    {
        //Events
        private static UnityAction _jumpHandler;
        private static UnityAction<GlideStages> _glideHandler;
        private static UnityAction _helpToggleHandler;

        //Private variables
        private ButtonConfig _buttonConfig;

        /// <summary>
        /// Stores the raw values of the input read from base axes.
        /// </summary>
        public static Vector2 InputRaw { get; private set; }

        private void Start()
        {
            _buttonConfig = ButtonConfig.GetInstance();
        }

        private void Update()
        {
            ChkAxes();
            ChkControls();
        }
        /// <summary>
        /// Checks the state of the axes.
        /// </summary>
        private void ChkAxes()
        {
            InputRaw = new Vector2(Input.GetAxisRaw(ButtonConfig.HorizontalAxisName), Input.GetAxisRaw(ButtonConfig.VerticalAxisName));
        }
        /// <summary>
        /// Checks whether any special buttons have been pressed. If yes - calls proper handlers, if these are not null.
        /// </summary>
        private void ChkControls()
        {
            if (Input.GetKeyDown(_buttonConfig.JumpButton))
            {
                _jumpHandler?.Invoke();
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
        }
        /// <summary>
        /// Registers jump handler.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterJumpHandler(UnityAction handler)
        {
            _jumpHandler += handler;
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
        /// Registers glide handler.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterHelpToggleHandler(UnityAction handler)
        {
            _helpToggleHandler += handler;
        }
    }
}
