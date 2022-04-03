using Assets.Scripts.Game.Common.Data.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.GUI.Common
{
    /// <summary>
    /// Controls progress bars.
    /// </summary>
    public class ProgressBarController : MonoBehaviour
    {
        [SerializeField]
        private Image _loadingBar;
        public bool IsActivated { get; private set; } = false;

        private Color _loadingBarColor;
        private void Start()
        {
            _loadingBarColor = _loadingBar.color;
        }
        /// <summary>
        /// Shows the progress bar and begins counting time.
        /// </summary>
        public void ShowProgressBar()
        {
            _loadingBar.color = _loadingBarColor;
            IsActivated = true;
        }
        public void HideProgressBar()
        {
            _loadingBar.color = SGConstants.TransparentColor;
            IsActivated = false;
        }
        /// <summary>
        /// Sets progress of the loading bar.
        /// </summary>
        /// <param name="progress">Values from 0 to 1.</param>
        public void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            _loadingBar.fillAmount = progress;
        }
    }
}
