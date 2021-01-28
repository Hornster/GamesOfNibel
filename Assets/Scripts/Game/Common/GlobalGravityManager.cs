using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Stores base information about gravity. Other classes can query it for the values.
    /// </summary>
    public class GlobalGravityManager : MonoBehaviour
    {
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

        private static GlobalGravityManager _instance;

        private void Awake()
        {
            _instance = this;
            _baseGravityValue = (2 * _baseJumpHeight) / (_baseJumpTime * _baseJumpTime);
        }
        /// <summary>
        /// Gets the base jump time.
        /// </summary>
        /// <returns></returns>
        public static float GetBaseJumpTime()
        {
            if (_instance == null)
            {
                Debug.LogError("ERROR: No Global gravity manager declared in scene!");
            }
            return _instance._baseJumpTime;
        }
        /// <summary>
        /// Gets the base jump height.
        /// </summary>
        /// <returns></returns>
        public static float GetBaseJumpHeight()
        {
            if (_instance == null)
            {
                Debug.LogError("ERROR: No Global gravity manager declared in scene!");
            }
            return _instance._baseJumpHeight;
        }
        /// <summary>
        /// Gets the base gravity value.
        /// </summary>
        /// <returns></returns>
        public static float GetBaseGravityValue()
        {
            if (_instance == null)
            {
                Debug.LogError("ERROR: No Global gravity manager declared in scene!");
            }
            return _instance._baseGravityValue;
        }
    }
}
