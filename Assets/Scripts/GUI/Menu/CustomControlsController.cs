using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.GUI.Menu.Interface;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Manages controls that are children of this scripts monobehavior.
    /// </summary>
    [RequireComponent(typeof(GUIMouseCaster))]
    public class CustomControlsController : MonoBehaviour
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
        /// Amount of controls connected to this controller. Iterated from 0!
        /// </summary>
        private int _controlsCount;
        /// <summary>
        /// Index of currently selected control.
        /// </summary>
        private int _currentSelectedIndex;
        /// <summary>
        /// Set to true if the control was switched for another one already. False when controls can be switched with
        /// axis-based input.
        /// </summary>
        private bool _controlSwitched;
        /// <summary>
        /// Set to true when the control was already pressed.
        /// </summary>
        private bool _controlAlreadyPressed;
        /// <summary>
        /// Is this script active?
        /// </summary>
        private bool _isActive;
        /// <summary>
        /// Stores all controls connected to this control, together with their IDs. All of them.
        /// </summary>
        private Dictionary<int, ICustomMenuControl> _currentlyUsedControls = new Dictionary<int, ICustomMenuControl>();
        /// <summary>
        /// Reference to the UI caster. Enables the controls to react to the mouse.
        /// </summary>
        private GUIMouseCaster _guiMouseCaster;

        private ICustomMenuControl _currentlyPointedAtControl;
        // Start is called before the first frame update
        void Start()
        {
            _guiMouseCaster = GetComponent<GUIMouseCaster>();

            _controlsCount = 0;

            //var controls = GetComponentsInChildren<ICustomMenuControl>();
            //foreach (var control in controls)
            //{
            //    _currentlyUsedControls.Add(_controlsCount, control);
            //    control.ControlId = _controlsCount;
            //    _controlsCount++;
            //}


            GUIInputReader.RegisterLMBPressedHandler(ChkMouseInput);
        }
        /// <summary>
        /// Selects control based on provided index.
        /// </summary>
        /// <param name="index"></param>
        private void SelectControl(int index)
        {
            var isButtonPresent = _currentlyUsedControls.TryGetValue(index, out var control);
            if (isButtonPresent)
            {
                control.SelectControl();
            }
        }
        /// <summary>
        /// Deselects control based on provided index.
        /// </summary>
        /// <param name="index"></param>
        private void DeselectControl(int index)
        {
            var isButtonPresent = _currentlyUsedControls.TryGetValue(index, out var control);
            if (isButtonPresent)
            {
                control.DeselectControl();
            }
        }
        /// <summary>
        /// Calls control press handler in control of given index.
        /// </summary>
        /// <param name="index">Index of pressed control.</param>
        private void PressControl(int index)
        {
            var isButtonPresent = _currentlyUsedControls.TryGetValue(index, out var control);
            if (isButtonPresent)
            {
                control.ControlPressed();
            }

            _controlAlreadyPressed = true;
        }
        /// <summary>
        /// Checks if the index is still within the available controls' range.
        /// </summary>
        private void ChkIndexRange()
        {
            if (_currentSelectedIndex >= _controlsCount)
            {
                _currentSelectedIndex = 0;
            }
            else if (_currentSelectedIndex < 0)
            {
                _currentSelectedIndex = _controlsCount - 1;//-1 since we count from 0
            }
        }
        
        /// <summary>
        /// Checks if the player moved the selection among the controls with use of vertical axis.
        /// </summary>
        private void ChkControlSelectionInput()
        {
            var currentInput = GUIInputReader.PlayerInput;
            if ((currentInput.y > _sensitivity || currentInput.x > _sensitivity) && _controlSwitched == false)
            {
                SwitchControls(_currentSelectedIndex - 1);
            }
            else if ((currentInput.y < -_sensitivity || currentInput.x < -_sensitivity) && _controlSwitched == false)

            {
                SwitchControls(_currentSelectedIndex + 1);
            }
            else if (ValueComparator.IsEqual(currentInput.y, 0.0f)
                    && ValueComparator.IsEqual(currentInput.x, 0.0f))
            {
                _controlSwitched = false;
            }
        }
        /// <summary>
        /// Checks if the player pressed selected control.
        /// </summary>
        private void ChkControlPressInput()
        {
            var currentInput = GUIInputReader.SubmitInput;
            if (ValueComparator.IsEqual(currentInput, 0.0f) == false && _controlAlreadyPressed == false)
            {
                PressControl(_currentSelectedIndex);
            }
            else if(ValueComparator.IsEqual(currentInput, 0.0f))
            {
                _controlAlreadyPressed = false;
            }
        }
        /// <summary>
        /// Checks if the pointer left the currently pointed at control area. If so, forces reaction on the old control
        /// and nulls it.
        /// </summary>
        private void PointerLeftCurrentControl()
        {
            if (_currentlyPointedAtControl != null)
            {
                var isButtonPresent = _currentlyUsedControls.TryGetValue(_currentSelectedIndex, out var control);
                if (isButtonPresent)
                {
                    control.PointerLeftControl();
                }

                _currentlyPointedAtControl = null;
            }
        }
        /// <summary>
        /// Checks the mouse position and forces selection changes accordingly.
        /// </summary>
        /// <param name="isPressed">Is the left mouse control pressed (submit control)?</param>
        private void ChkMouseInput(bool isPressed = false)
        {
            var castResult = _guiMouseCaster.CastUIRayFromMouse();

            if (!castResult.isValid) return;

            var hitControl = castResult.gameObject.GetComponent<ICustomMenuControl>();

            if (hitControl == null)
            {
                PointerLeftCurrentControl();
                return;
            }

            _currentlyPointedAtControl = hitControl;
            int controlId = hitControl.ControlId;
            //TODO: add calling for pointer left. Remember last control that you can point at
            //and if during next check that control could not be found in the returned hit stack - call
            //pointerLeft for it before switching it.
            if (isPressed)
            {
                PressControl(controlId);
            }
            else
            {
                SwitchControls(controlId);
            }
        }
        void Update()
        {
            ChkControlSelectionInput();
            ChkControlPressInput();
            ChkMouseInput();
        }

        /// <summary>
        /// Deselects currently selected control and selects new one.
        /// </summary>
        /// <param name="newIndex">Index of a control that shall be selected.</param>
        public void SwitchControls(int newIndex)
        {
            if (_currentSelectedIndex != newIndex)
            {
                DeselectControl(_currentSelectedIndex);
            }

            //PointerLeftCurrentControl();
            _currentSelectedIndex = newIndex;
            ChkIndexRange();
            SelectControl(_currentSelectedIndex);
            _controlSwitched = true;
        }
        /// <summary>
        /// Event handler that allows for changing the controls this controller works with.
        /// </summary>
        /// <param name="_controlsFinder">New set of controls. Will override the old set in this controller.</param>
        public void ReplaceControls(CustomControlsFinder _controlsFinder)
        {
            _currentlyUsedControls = _controlsFinder.FoundControls;
            _controlsCount = _controlsFinder.ControlsCount;

            SelectControl(_currentSelectedIndex);
            _currentSelectedIndex = 0;
        }
    }
}
//TODO Every button that causes change of UI layout (transition) has small script that
//TODO gets the event from the control and sends event with parameters to the transition  manager.