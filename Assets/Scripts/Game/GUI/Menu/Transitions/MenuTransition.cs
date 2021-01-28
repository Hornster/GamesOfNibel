using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu.Transitions
{
    /// <summary>
    /// Manages transition for assigned menu.
    /// </summary>
    public class MenuTransition : MonoBehaviour
    {
        [Header("Parameters")]
        [Tooltip("How long does it take for the transition to finish.")]
        [SerializeField] private float _fadeInDuration = 0.5f;
        [Tooltip("How long does it take for the transition to finish.")]
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [Tooltip("Name of the fade in causing trigger in the animator.")]
        [SerializeField] private string _fadeInTriggerName = "fadeIn";
        [Tooltip("Name of the fade out causing trigger in the animator.")]
        [SerializeField] private string _fadeOutTriggerName = "fadeOut";

        [SerializeField]
        [Tooltip("What type of menu is connected with this controller.")]
        private MenuType _menuType;

        public MenuType MenuType => _menuType;

        [Header("Required references")]
        [Tooltip("Canvas group that steers the interactability and visibility of the menu.")]
        [SerializeField] private CanvasGroup _menuCanvasGroup;

        [SerializeField] private Animator _animator;

        [Header("Event handlers")]
        [Tooltip("Used to enable or disable given elements upon transition happening.")]
        [SerializeField]
        private BoolUnityEvent _setActiveComponents;
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
        /// <summary>
        /// Makes the menu invisible, disables raycasts blocking and interactability for the menu.
        /// </summary>
        public void HideMenu()
        {
            _menuCanvasGroup.alpha = 0.0f;
            _menuCanvasGroup.blocksRaycasts = false;
            _menuCanvasGroup.interactable = false;
            _setActiveComponents?.Invoke(false);
        }
        /// <summary>
        /// Makes the menu visible, enables raycasts and interactability for the menu.
        /// </summary>
        public void ShowMenu()
        {
            _menuCanvasGroup.alpha = 1.0f;
            _menuCanvasGroup.blocksRaycasts = true;
            _menuCanvasGroup.interactable = true;
            _setActiveComponents?.Invoke(true);
        }

    }
}
