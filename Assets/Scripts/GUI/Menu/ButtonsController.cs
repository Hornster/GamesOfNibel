using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.GUI.Menu.Interface;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Manages buttons that are children of this scripts monobehavior.
    /// </summary>
    [RequireComponent(typeof(GUIMouseCaster))]
    public class ButtonsController : MonoBehaviour
    {
        /// <summary>
        /// The sensitivity of the player input, ranges from 0 to 1.
        /// Player input value on axis must be bigger (absolute values) than the sensitivity
        /// for it to be interpreted as an action.
        /// </summary>
        [SerializeField]
        [Range(0f, 1f)]
        private float _sensitivity;
        /// <summary>
        /// Amount of buttons connected to this controller. Iterated from 0!
        /// </summary>
        private int _buttonsCount;
        /// <summary>
        /// Index of currently selected button.
        /// </summary>
        private int _currentSelectedIndex;
        /// <summary>
        /// Set to true if the button was switched for another one already. False when buttons can be switched with
        /// axis-based input.
        /// </summary>
        private bool _buttonSwitched;
        /// <summary>
        /// Set to true when the button was already pressed.
        /// </summary>
        private bool _buttonAlreadyPressed;
        /// <summary>
        /// Stores all buttons connected to this control, together with their IDs. All of them.
        /// </summary>
        private Dictionary<int, ICustomMenuControl> _buttons = new Dictionary<int, ICustomMenuControl>();
        /// <summary>
        /// Reference to the UI caster. Enables the controls to react to the mouse.
        /// </summary>
        private GUIMouseCaster _guiMouseCaster;
        // Start is called before the first frame update
        void Start()
        {
            _guiMouseCaster = GetComponent<GUIMouseCaster>();

            _buttonsCount = 0;

            var buttons = GetComponentsInChildren<ICustomMenuControl>();
            foreach (var button in buttons)
            {
                _buttons.Add(_buttonsCount, button);
                button.ControlId = _buttonsCount;
                _buttonsCount++;
            }

            _currentSelectedIndex = 0;
            SelectButton(_currentSelectedIndex);

            GUIInputReader.RegisterLMBPressedHandler(ChkMouseInput);
        }
        /// <summary>
        /// Selects button based on provided index.
        /// </summary>
        /// <param name="index"></param>
        private void SelectButton(int index)
        {
            var isButtonPresent = _buttons.TryGetValue(index, out var button);
            if (isButtonPresent)
            {
                button.SelectControl();
            }
        }
        /// <summary>
        /// Deselects button based on provided index.
        /// </summary>
        /// <param name="index"></param>
        private void DeselectButton(int index)
        {
            var isButtonPresent = _buttons.TryGetValue(index, out var button);
            if (isButtonPresent)
            {
                button.DeselectControl();
            }
        }
        /// <summary>
        /// Calls button press handler in button of given index.
        /// </summary>
        /// <param name="index">Index of pressed button.</param>
        private void PressButton(int index)
        {
            var isButtonPresent = _buttons.TryGetValue(index, out var button);
            if (isButtonPresent)
            {
                button.ControlPressed();
            }

            _buttonAlreadyPressed = true;
        }
        /// <summary>
        /// Checks if the index is still within the available buttons' range.
        /// </summary>
        private void ChkIndexRange()
        {
            if (_currentSelectedIndex >= _buttonsCount)
            {
                _currentSelectedIndex = 0;
            }
            else if (_currentSelectedIndex < 0)
            {
                _currentSelectedIndex = _buttonsCount - 1;//-1 since we count from 0
            }
        }
        /// <summary>
        /// Deselects currently selected button and selects new one.
        /// </summary>
        /// <param name="change">The amount by which the selection shall jump.</param>
        private void SwitchButtons(int newIndex)
        {
            DeselectButton(_currentSelectedIndex);
            _currentSelectedIndex = newIndex;
            ChkIndexRange();
            SelectButton(_currentSelectedIndex);
            _buttonSwitched = true;
        }
        /// <summary>
        /// Checks if the player moved the selection among the buttons with use of vertical axis.
        /// </summary>
        private void ChkButtonSelectionInput()
        {
            var currentInput = GUIInputReader.PlayerInput;
            if (currentInput.y > _sensitivity && _buttonSwitched == false)
            {
                SwitchButtons(_currentSelectedIndex - 1);
            } else if(currentInput.y < -_sensitivity && _buttonSwitched == false)

            {
                SwitchButtons(_currentSelectedIndex + 1);
            }
            else if(ValueComparator.IsEqual(currentInput.y, 0.0f))
            {
                _buttonSwitched = false;
            }
        }
        /// <summary>
        /// Checks if the player pressed selected button.
        /// </summary>
        private void ChkButtonPressInput()
        {
            var currentInput = GUIInputReader.SubmitInput;
            if (ValueComparator.IsEqual(currentInput, 0.0f) == false && _buttonAlreadyPressed == false)
            {
                PressButton(_currentSelectedIndex);
            }
            else if(ValueComparator.IsEqual(currentInput, 0.0f))
            {
                _buttonAlreadyPressed = false;
            }
        }
        /// <summary>
        /// Checks the mouse position and forces selection changes accordingly.
        /// </summary>
        /// <param name="isPressed">Is the left mouse button pressed (submit button)?</param>
        private void ChkMouseInput(bool isPressed = false)
        {
            var castResult = _guiMouseCaster.CastUIRayFromMouse();

            if (!castResult.isValid) return;

            var hitControl = castResult.gameObject.GetComponent<ICustomMenuControl>();

            if (hitControl == null) return;

            int controlId = hitControl.ControlId;

            if (isPressed)
            {
                PressButton(controlId);
            }
            else
            {
                SwitchButtons(controlId);
            }
        }
        void Update()
        {
            ChkButtonSelectionInput();
            ChkButtonPressInput();
            ChkMouseInput();
        }
    }
}
