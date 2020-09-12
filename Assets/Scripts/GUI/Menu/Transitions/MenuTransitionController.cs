using UnityEngine;

namespace Assets.Scripts.GUI.Menu.Transitions
{
    /// <summary>
    /// Manages transition for assigned menu.
    /// </summary>
    public class MenuTransitionController : MonoBehaviour
    {
        [Tooltip("How long does it take for the transition to finish.")]
        [SerializeField] private float _fadeInDuration = 0.5f;
        [Tooltip("How long does it take for the transition to finish.")]
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [Tooltip("Name of the fade in causing trigger in the animator.")]
        [SerializeField] private string _fadeInTriggerName = "fadeIn";
        [Tooltip("Name of the fade out causing trigger in the animator.")]
        [SerializeField] private string _fadeOutTriggerName = "fadeOut";

        [SerializeField] private Animator _animator;
        /// <summary>
        /// Causes fade in effect to occur.
        /// </summary>
        /// <returns></returns>
        public float FadeIn()
        {
            _animator.ResetTrigger(_fadeOutTriggerName);
            _animator.SetTrigger(_fadeInTriggerName);
            return _fadeInDuration;
        }
        /// <summary>
        /// Causes fade out effect to occur.
        /// </summary>
        /// <returns></returns>
        public float FadeOut()
        {
            _animator.ResetTrigger(_fadeInTriggerName);
            _animator.SetTrigger(_fadeOutTriggerName);
            return _fadeOutDuration;
        }

    }
}
