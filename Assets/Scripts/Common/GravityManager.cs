using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class GravityManager :MonoBehaviour
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
        /// What default gravity value affects the target?
        /// </summary>
        private float _referenceGravityValue;
        /// <summary>
        /// Which rigidbody is this manager influencing. This is done automatically each FixedUpdate pass.
        /// </summary>
        [SerializeField] private Rigidbody2D _influencedRigidbody;
        /// <summary>
        /// Defines current value by which the gravity shall be multiplied the next frame.
        /// </summary>
        private float _currentGravityMultiplier = 1.0f;
        /// <summary>
        /// Current value of Gravity. Assigned to the target rigidbody.
        /// </summary>
        private float _currentGravityValue;

        private ListDictionary _activeMultipliers = new ListDictionary();

        private void Awake()
        {
            _referenceGravityValue = (2 * _baseJumpHeight) / (_baseJumpTime * _baseJumpTime);
            _currentGravityValue = _referenceGravityValue;
            Debug.Log("Resetting the " + _influencedRigidbody + " to 0.");
            _influencedRigidbody.gravityScale = 0.0f;
        }
        /// <summary>
        /// Recalculates the current gravity multiplier basing of stored multipliers.
        /// </summary>
        private void CalcGravityMultiplier()
        {
            //Reset the gravity multiplier.
            _currentGravityMultiplier = 1.0f;
            //Then calculate its value.
            foreach (var modificator in _activeMultipliers.Values)
            {
                _currentGravityMultiplier *= (float)modificator;
            }
        }

        /// <summary>
        /// Removes all gravity modifiers.
        /// </summary>
        private void ResetGravityModifiers()
        {
            _activeMultipliers.Clear();
        }

        private void FixedUpdate()
        {
            CalcGravityMultiplier();

            _currentGravityValue = _referenceGravityValue * _currentGravityMultiplier;
            var newVelocityY = _influencedRigidbody.velocity.y - _currentGravityValue * Time.deltaTime;

            _influencedRigidbody.velocity = new Vector2(_influencedRigidbody.velocity.x, newVelocityY); ;

            ResetGravityModifiers();
        }
        /// <summary>
        /// Applies a multiplier to the reference gravity value. Every object can apply only one modifier at a time.
        /// </summary>
        /// <param name="value">The value applied to the gravity multiplier.</param>
        /// <param name="addingObjRefHash">The hash of the object that adds the value. Used as key.</param>
        public void AddGravityModifier(float value, int addingObjRefHash)
        {
            if (_activeMultipliers.Contains(addingObjRefHash) == false)
            {
                _activeMultipliers.Add(addingObjRefHash, value);
                _currentGravityMultiplier *= value;
            }
        }
        /// <summary>
        /// Returns the reference value of the gravity (not influenced by any factors).
        /// </summary>
        /// <returns></returns>
        public float GetRefGravityValue()
        {
            return _referenceGravityValue;
        }
        /// <summary>
        /// Calculates and returns base jump beginning velocity.
        /// </summary>
        /// <returns></returns>
        public float GetBaseJumpStartVelocity()
        {
            return _baseJumpHeight / _baseJumpTime + 0.5f * _referenceGravityValue * _baseJumpTime;
        }
        /// <summary>
        /// Returns the time it takes the character to reach higher point in regular jump, in seconds.
        /// </summary>
        /// <returns></returns>
        public float GetBaseJumptTime()
        {
            return _baseJumpTime;
        }
    }
}
