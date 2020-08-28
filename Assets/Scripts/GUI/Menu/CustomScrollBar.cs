using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.GUI.Menu.Interface;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Manages single scrollbar. Can be used with Scrollbar component in Unity.
    /// </summary>
    public class CustomScrollBar : MonoBehaviour, ICustomMenuControl
    {

        /// <summary>
        /// All events that shall be called upon clicking the scrollbar.
        /// </summary>
        [Header("Required references")]
        [SerializeField] private UnityEvent _barPressedEvents;
        /// <summary>
        /// Used to force selection of this control to the controls controller. Useful when more than one
        /// element can make the control become selected. Passes ID of this control.
        /// </summary>
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

        private void Start()
        {
            var controlsController = GetComponentInParent<CustomControlsController>();
            _forcedSelectionEvent += controlsController.SwitchControls;
        }
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
        /// Forces selection of this control by invoking event handler. Make sure you connected the correct
        /// handler(s).
        /// </summary>
        public void SelectThisControl()
        {
            Debug.Log($"Forced selection event for scrollbar!");
            _forcedSelectionEvent?.Invoke(ControlId);
        }
    }
}
