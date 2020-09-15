using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GUI.Menu.Interface;
using Assets.Scripts.Mods.Maps;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Defines a menu control that can be selected by the user.
    /// </summary>
    public class CustomSelectableControl : MonoBehaviour, ICustomMenuControl
    {
        /// <summary>
        /// Called when this control is selected (pointed at).
        /// </summary>
        [SerializeField]
        private UnityEvent _onSelectedEvent;
        /// <summary>
        /// Called when the control has been selected by clicking on it.
        /// Provides its data.
        /// </summary>
        private UnityAction<CustomSelectableControl> _onSelected;
        /// <summary>
        /// Called when the control was hovered over. Provides data of assigned to this control map.
        /// </summary>
        private UnityAction<CustomSelectableControl> _onPointedAt;
        /// <summary>
        /// Called when the pointer left the control's area.
        /// </summary>
        private UnityAction _onStoppedPointing;
        /// <summary>
        /// All events that shall be called upon clicking the button.
        /// </summary>
        [Header("Required references")]
        [SerializeField] private UnityEvent _assignedEvents;
        /// <summary>
        /// Reference to the animator component.
        /// </summary>
        [SerializeField] private Animator _animator;
        /// <summary>
        /// Data describing map assigned to this control.
        /// </summary>
        [SerializeField] private MapDataGameObjectAdapter _mapData;
        /// <summary>
        /// Used to toggle the selection presenting shader.
        /// </summary>
        [SerializeField] private ControlShaderDisabler _shaderDisabler;

        [Header("Animator params")]
        [SerializeField]
        private string _idleParamName = "idle";
        [SerializeField]
        private string _pointedAtParamName = "pointedAt";
        [SerializeField]
        private string _pressedParamName = "pressed";

        public int ControlId { get; set; }
        /// <summary>
        /// Retrieves the map data assigned to this control.
        /// </summary>
        public MapData MapData => _mapData.MapData;
        public void DeselectControl()
        {
            _animator.SetBool(_idleParamName, true);
            _animator.SetBool(_pointedAtParamName, false);
            _animator.ResetTrigger(_pressedParamName);
            Debug.Log($"{ControlId} was deselected.");
        }

        public void SelectControl()
        {
            _animator.SetBool(_idleParamName, false);
            _animator.SetBool(_pointedAtParamName, true);
            _onPointedAt?.Invoke(this);
            Debug.Log($"{ControlId} was selected.");
        }

        public void ControlPressed()
        {
            _animator.SetTrigger(_pressedParamName);
            _assignedEvents?.Invoke();
            _onSelected?.Invoke(this);
            _shaderDisabler.EnableShader();
            Debug.Log($"{ControlId} was pressed.");
        }
        /// <summary>
        /// Calls the _onStoppedPointing event handler.
        /// </summary>
        public void PointerLeftControl()
        {
            _onStoppedPointing?.Invoke();
        }

        /// <summary>
        /// Used to disable selection of this selectable control. (selection after pressing it)
        /// </summary>
        public void DisableSelection()
        {
            _shaderDisabler.DisableShader();
        }

        public void RegisterOnSelected(UnityAction handler)
        {
            _onSelectedEvent.AddListener(handler);
        }
        public void RegisterOnSelected(UnityAction<CustomSelectableControl> handler)
        {
            _onSelected += handler;
        }
        public void RegisterOnPointedAt(UnityAction<CustomSelectableControl> handler)
        {
            _onPointedAt += handler;
        }
        public void RegisterOnStoppedPointingAt(UnityAction handler)
        {
            _onStoppedPointing += handler;
        }
    }
}
