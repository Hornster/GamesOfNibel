using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GUI.Menu.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Manages single skill toggle, allowing it to change colors accordingly when needed.
    /// </summary>
    public class CustomSkillToggle : MonoBehaviour, ICustomMenuControl
    {
        /// <summary>
        /// All events that shall be called upon clicking the button.
        /// </summary>
        [SerializeField] private UnityEvent _assignedEvents;
        /// <summary>
        /// Called when the button has been stopped being pointed at.
        /// </summary>
        [SerializeField] private UnityEvent _onStoppedPointingAt;
        /// <summary>
        /// Reference to the animator component.
        /// </summary>
        [Header("Required references")]
        [SerializeField] private Animator _animator;
        /// <summary>
        /// Reference to the skill icon that shall have the color updated.
        /// </summary>
        [SerializeField] private Image _skillIcon;
        /// <summary>
        /// Scriptable data object that stores colors for the icons.
        /// </summary>
        [SerializeField] private SkillMenuColors _skillMenuColors;

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

        /// <summary>
        /// For regular switch control does the same as deselect.
        /// </summary>
        public void PointerLeftControl()
        {
            _onStoppedPointingAt?.Invoke();
        }

        /// <summary>
        /// Changes the color of this control to provided one.
        /// </summary>
        /// <param name="newSkillColor"></param>
        public void ChangeColor(MenuSkillColor newSkillColor)
        {
            switch(newSkillColor)
            {
                case MenuSkillColor.Enabled:
                    _skillIcon.color = _skillMenuColors.EnabledSkill;
                    break;
                case MenuSkillColor.Disabled:
                    _skillIcon.color = _skillMenuColors.DisabledSkill;
                    break;
                case MenuSkillColor.RequiredEnabled:
                    _skillIcon.color = _skillMenuColors.EnabledSkillNeeded;
                    break;
                case MenuSkillColor.RequiredDisabled:
                    _skillIcon.color = _skillMenuColors.DisabledSkillNeeded;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newSkillColor), newSkillColor, null);
            }
        }
    }
}
