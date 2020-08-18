using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Contains methods and fields necessary to reset the animation machine state.
    /// </summary>
    public class ButtonAnimationReseter : MonoBehaviour
    {
        /// <summary>
        /// Reference to the animator component.
        /// </summary>
        [SerializeField] private Animator _animator;
        [SerializeField]
        private string _pressedParamName = "pressed";
        /// <summary>
        /// Called at the end of button pressing animation to change the animation state back to selected.
        /// </summary>
        private void ButtonAnimPressedEnded()
        {
            _animator.SetBool(_pressedParamName, false);
        }
    }
}
