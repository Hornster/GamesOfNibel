using Assets.Scripts.GUI.Menu.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Manages single button.
    /// </summary>
    public class CustomButton : MonoBehaviour, ICustomMenuControl
    {
        /// <summary>
        /// All events that shall be called upon clicking the button.
        /// </summary>
        [Header("Required references")]
        [SerializeField] private UnityEvent _assignedEvents;
        /// <summary>
        /// Reference to the animator component.
        /// </summary>
        [SerializeField] private Animator _animator;

        [Header("Animator params")] 
        [SerializeField]
        private string _idleParamName = "idle";
        [SerializeField]
        private string _selectedParamName = "selected";
        [SerializeField]
        private string _pressedParamName = "pressed";
        /// <summary>
        /// The ID of this control. Ought to be unique in the controller that this control belongs to.
        /// </summary>
        public int ControlId { get; set; }
        /// <summary>
        /// Deselects the button.
        /// </summary>
        public void DeselectControl()
        {
            _animator.SetBool(_idleParamName, true);
            _animator.SetBool(_selectedParamName, false);
        }
        /// <summary>
        /// Selects the button.
        /// </summary>
        public void SelectControl()
        {
            _animator.SetBool(_idleParamName, false);
            _animator.SetBool(_selectedParamName, true);
        }
        /// <summary>
        /// Called upon pressing the button.
        /// </summary>
        public void ControlPressed()
        {
            _animator.SetBool(_pressedParamName, true);
            _assignedEvents?.Invoke();
        }
    }
}
