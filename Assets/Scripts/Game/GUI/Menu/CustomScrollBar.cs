﻿using Assets.Scripts.Game.GUI.Menu.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.GUI.Menu
{
    /// <summary>
    /// Manages single scrollbar. Can be used with Scrollbar component in Unity.
    /// </summary>
    public class CustomScrollBar : MonoBehaviour, ICustomMenuControl
    {
        /// <summary>
        /// All events that shall be called upon clicking the scrollbar.
        /// </summary>
        [SerializeField] private UnityEvent _barPressedEvents;
        /// <summary>
        /// Called when the button has been stopped being pointed at.
        /// </summary>
        [SerializeField] private UnityEvent _onStoppedPointingAt;
        /// <summary>
        /// Used to force selection of this control to the controls controller. Useful when more than one
        /// element can make the control become selected. Passes ID of this control.
        /// </summary>
        [Header("Required references")]
        private UnityAction<int> _forcedSelectionEvent;
        /// <summary>
        /// Reference to the animator component.
        /// </summary>
        [SerializeField] private Animator _animator;

        [Header("Animator params")]
        [SerializeField]
        private string _isSelectedParamName = "IsSelected";
        /// <summary>
        /// The ID of this control. Ought to be unique in the controller that this control belongs to.
        /// </summary>
        public int ControlId { get; set; }

        public void DeselectControl()
        {
            _animator.SetBool(_isSelectedParamName, false);
        }

        public void SelectControl()
        {
            _animator.SetBool(_isSelectedParamName, true);
        }

        public void ControlPressed()
        {
            _barPressedEvents?.Invoke();
        }
        /// <summary>
        /// For regular slider does the same as deselect.
        /// </summary>
        public void PointerLeftControl()
        {
            _onStoppedPointingAt?.Invoke();
        }

    }
}
