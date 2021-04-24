using Assets.Scripts.Game.Common.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.Scripts.Game.GUI
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
        /// Called when cancelling control, like escape key, has been pressed.
        /// </summary>
        private static UnityAction _onCancelEvent;
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

        private bool _cancelEventTriggered = false;
        private void Update()
        {
            PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            SubmitInput = Input.GetAxisRaw("Submit");
            CancelInput = Input.GetAxisRaw("Cancel");
            MousePosition = Input.mousePosition;

            ChkEventInput();
            UpdateAxisToEvent();
        }
        /// <summary>
        /// Updates state of inputs that are made of axes read and converts them to event calls. Thus, given
        /// axis-based trigger is called only once.
        /// </summary>
        private void UpdateAxisToEvent()
        {
            if (ValueComparator.IsEqual(CancelInput, 0f) == false)
            {
                if (_cancelEventTriggered == false)
                {
                    _onCancelEvent?.Invoke();
                    _cancelEventTriggered = true;
                    Debug.Log("Event triggered!");
                }
            }
            else
            {
                _cancelEventTriggered = false;
            }
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
        /// <summary>
        /// Registers handler for cancel operation.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterOnCancelHandler(UnityAction handler)
        {
            _onCancelEvent += handler;
        }

        private void OnDestroy()
        {
            _LMBPressed = null;
            _onCancelEvent = null;
        }
    }
}
