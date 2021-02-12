using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.Game.Player;
using Cinemachine;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Camera
{
    /// <summary>
    /// Causes camera to move around by moving observed object.
    /// </summary>
    public class PlayerIdleCameraAnimator : MonoBehaviour
    {
        [Tooltip("Object that will be moved around.")]
        [SerializeField]
        private Transform _objectToMove;
        [TextArea]
        public string _objectToMoveNotes = "MovementBoundingBox shall have it's pivot in the very middle.";
        [Tooltip("Boundaries for the X and Y movement of the camera while player idling.")]
        [SerializeField]
        private RectTransform _movementBoundingBox;
        [Tooltip("Time it takes for the camera to fully sway back and forth once, in seconds.")]
        [SerializeField]
        private Vector2 _movementTime = new Vector2(1f, 1f);

        [Tooltip("Used to retrieve player's current velocity.")]
        [SerializeField] private PlayerState _playerState;
        /// <summary>
        /// Base value, for 1 second sway (for example one full sine cycle). 360 degrees.
        /// </summary>
        private float _baseScale = 360f;

        private bool _positionWasReset = false;

        private void Start()
        {
            //Reset the position of the object that will be moved.
            ResetMovingObjectPosition();
        }

        private void ResetMovingObjectPosition()
        {
            _objectToMove.position = _movementBoundingBox.position;
            _positionWasReset = true;
        }

        /// <summary>
        /// Calculates translation vector for the camera idle movement.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        private Vector2 CalculateMovementVector(float deltaTime)
        {
            float xAngle = deltaTime * _baseScale / _movementTime.x;
            float yAngle = deltaTime * _baseScale / _movementTime.y;
            float xTranslation = (float)Math.Sin(xAngle) * _movementBoundingBox.rect.width;
            float yTranslation = (float)Math.Cos(yAngle) * _movementBoundingBox.rect.height;

            return new Vector2(xTranslation, yTranslation);
        }

        private void PerformTranslation(Vector2 translation)
        {
            _objectToMove.position = _movementBoundingBox.position - (Vector3)_movementBoundingBox.rect.size/2 + (Vector3)translation;
        }

        private void FixedUpdate()
        {
            var playerCurrentVelocity = _playerState.NewVelocity;
            if (ValueComparator.IsEqual(playerCurrentVelocity.x, 0f) == false ||
                ValueComparator.IsEqual(playerCurrentVelocity.y, 0f) == false)
            {
                if (_positionWasReset == false)
                {
                    ResetMovingObjectPosition();//We need to reset the position only once.
                }
                
                return; //If player is moving, simply return.
            }
            //Else apply movement of the object.
            float deltaTime = Time.fixedDeltaTime;
            var translation = CalculateMovementVector(deltaTime);
            PerformTranslation(translation);
        }
    }
}
//TODO - find a way to add scalable, readable field in Unity that defines the rect of idle cam movement.