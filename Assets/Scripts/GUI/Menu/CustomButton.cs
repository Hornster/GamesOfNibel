using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Manages single button.
    /// </summary>
    public class CustomButton : MonoBehaviour
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
        /// Deselects the button.
        /// </summary>
        public void DeselectButton()
        {
            _animator.SetBool(_idleParamName, true);
            _animator.SetBool(_selectedParamName, false);
        }
        /// <summary>
        /// Selects the button.
        /// </summary>
        public void SelectButton()
        {
            _animator.SetBool(_idleParamName, false);
            _animator.SetBool(_selectedParamName, true);
        }
        /// <summary>
        /// Called upon pressing the button.
        /// </summary>
        public void ButtonPressed()
        {
            _animator.SetBool(_pressedParamName, true);
            _assignedEvents?.Invoke();
        }
        /// <summary>
        /// Called at the end of button pressing animation to change the animation state back to selected.
        /// </summary>
        private void ButtonAnimPressedEnded()
        {
            _animator.SetBool(_pressedParamName, false);
            //_animator.SetBool(_selectedParamName, true);
        }
    }
}
