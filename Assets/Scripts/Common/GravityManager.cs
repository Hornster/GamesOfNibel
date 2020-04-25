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
        /// What default gravity value affects the target?
        /// </summary>
        [SerializeField] private float _referenceGravityValue;
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

        private void Start()
        {
            _currentGravityValue = _referenceGravityValue;
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
            _influencedRigidbody.gravityScale = _currentGravityValue;

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
        

    }
}
