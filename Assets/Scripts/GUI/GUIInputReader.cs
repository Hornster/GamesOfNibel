using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.Scripts.GUI
{
    /// <summary>
    /// Static class for reading the input data from the user when in GUI.
    /// </summary>
    public class GUIInputReader : MonoBehaviour
    {
        /// <summary>
        /// Handler for event where left mouse button has been pressed once.
        /// </summary>
        private static UnityAction<bool> _LMBPressed;
        /// <summary>
        /// Stores raw input from the player on horizontal and vertical axes. Updated as fast as Unity can.
        /// </summary>
        public static Vector2 PlayerInput { get; private set; }
        /// <summary>
        /// Stores position of the mouse on the screen. Updated as fast as Unity can.
        /// </summary>
        public static Vector3 MousePosition { get; private set; }
        /// <summary>
        /// Stores raw input from submit control.
        /// </summary>
        public static float SubmitInput { get; private set; }
        /// <summary>
        /// Stores raw input from cancel control.
        /// </summary>
        public static float CancelInput { get; private set; }

        private void Update()
        {
            PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            SubmitInput = Input.GetAxisRaw("Submit");
            CancelInput = Input.GetAxisRaw("Cancel");
            MousePosition = Input.mousePosition;

            ChkEventInput();
        }

        private void ChkEventInput()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                _LMBPressed?.Invoke(true);
            }
        }
        /// <summary>
        /// Registers handler for the left mouse button press event.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterLMBPressedHandler(UnityAction<bool> handler)
        {
            _LMBPressed += handler;
        }
    }
}
