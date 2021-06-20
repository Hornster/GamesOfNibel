using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Stores base information about gravity. Other classes can query it for the values.
    /// </summary>
    public class GlobalGravityManager : SceneSingleton<GlobalGravityManager>
    {
        [Header("")]
        /// <summary>
        /// How long will it take to reach max height jumping off the ground vertically.
        /// </summary>
        [SerializeField]
        private float _baseJumpTime = 1.5f;
        /// <summary>
        /// How high can the character jump from the ground vertically.
        /// </summary>
        [SerializeField]
        private float _baseJumpHeight = 3.0f;

        /// <summary>
        /// The base gravity value.
        /// </summary>
        private float _baseGravityValue;
        /// <summary>
        /// Set to true when Awake has been called at least once.
        /// </summary>
        private static bool IsInitialized = false;

        //private static GlobalGravityManager _instance;

        private void Awake()
        {
            _baseGravityValue = (2 * _baseJumpHeight) / (_baseJumpTime * _baseJumpTime);
            IsInitialized = true;
        }
        /// <summary>
        /// Checks if the Awake method has been called. If not - calls it.
        /// </summary>
        public void ChkIfInitialized()
        {
            if (IsInitialized == false)
            {
                Awake();
            }
        }
        /// <summary>
        /// Gets the base jump time.
        /// </summary>
        /// <returns></returns>
        public float GetBaseJumpTime()
        {
            ChkIfInitialized();
            return Instance._baseJumpTime;
        }
        /// <summary>
        /// Gets the base jump height.
        /// </summary>
        /// <returns></returns>
        public float GetBaseJumpHeight()
        {
            ChkIfInitialized();
            return Instance._baseJumpHeight;
        }
        /// <summary>
        /// Gets the base gravity value.
        /// </summary>
        /// <returns></returns>
        public float GetBaseGravityValue()
        {
            ChkIfInitialized();
            return Instance._baseGravityValue;
        }
    }
}
