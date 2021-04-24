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
        [Tooltip("Camera that is active when the player is moving.")]
        [SerializeField]
        private CinemachineVirtualCamera _movementVirtualCamera;
        [Tooltip("Camera that is active when the player is not moving.")]
        [SerializeField]
        private CinemachineVirtualCamera _idleVirtualCamera;
        [TextArea]
        public string _objectToMoveNotes = "MovementBoundingBox shall have it's pivot in the very middle.";
        [Tooltip("Boundaries for the X and Y movement of the camera while player idling.")]
        [SerializeField]
        private RectTransform _movementBoundingBox;
        [Tooltip("Time it takes for the camera to fully sway back and forth once, in seconds.")]
        [SerializeField]
        private Vector2 _movementTime = new Vector2(1f, 1f);

        [Tooltip("Amount of time it takes for the idle camera animation to kick in.")]
        [SerializeField] private float _idleEnableTime = 1f;

        [Tooltip("Used to retrieve player's current velocity.")]
        [SerializeField] private PlayerState _playerState;
        /// <summary>
        /// Base value, for 1 second sway (for example one full sine cycle). 360 degrees, in radians.
        /// </summary>
        private float _baseScale = 2*(float)Math.PI;

        private bool _camerasSwitched = false;

        /// <summary>
        /// Total amount of time that has passed since this script's launch.
        /// </summary>
        private float _totalTime = 0f;
        /// <summary>
        /// Used for counting the time for the idle camera animation to start.
        /// </summary>
        private float _idleEnableCounter = 0f;

        /// <summary>
        /// The priority of active camera out of the two.
        /// </summary>
        private int _higherPriority;
        /// <summary>
        /// The priority of not active camera out of the two.
        /// </summary>
        private int _lowerPriority;

        private bool _isMovementCamActive;

        private void Start()
        {
            _higherPriority = Math.Max(_movementVirtualCamera.Priority, _idleVirtualCamera.Priority);
            _lowerPriority = Math.Min(_movementVirtualCamera.Priority, _idleVirtualCamera.Priority);
            //Reset the position of the object that will be moved.
            ResetMovingObjectPosition();
            EnableIdleCam();
        }

        private void ResetMovingObjectPosition()
        {
            _objectToMove.position = _movementBoundingBox.position;
            _camerasSwitched = true;
        }

        /// <summary>
        /// Calculates translation vector for the camera idle movement.
        /// </summary>
        /// <returns></returns>
        private Vector2 CalculateMovementVector()
        {
            float xAngle = _totalTime * _baseScale / _movementTime.x;
            float yAngle = _totalTime * _baseScale / _movementTime.y;
            float xTranslation = (float)Math.Sin(xAngle) * _movementBoundingBox.rect.width/2;
            float yTranslation = (float)Math.Cos(yAngle) * _movementBoundingBox.rect.height/2;

            return new Vector2(xTranslation, yTranslation);
        }

        private void PerformTranslation(Vector2 translation)
        {
            _objectToMove.position = _movementBoundingBox.position + (Vector3)translation;
        }

        /// <summary>
        /// Enables movement cam and disables idle cam.
        /// </summary>
        private void EnableMovementCam()
        {
            _movementVirtualCamera.Priority = _higherPriority;
            _idleVirtualCamera.Priority = _lowerPriority;
            _isMovementCamActive = true;
        }
        /// <summary>
        /// Enables idle cam and disables movement cam.
        /// </summary>
        private void EnableIdleCam()
        {
            _movementVirtualCamera.Priority = _lowerPriority;
            _idleVirtualCamera.Priority = _higherPriority;
            _isMovementCamActive = false;
        }
        

        private bool IsIdleAnimEnabled(float deltaTime)
        {
            _idleEnableCounter += deltaTime;
            return _idleEnableCounter >= _idleEnableTime;
        }

        private void FixedUpdate()
        {
            var playerCurrentVelocity = _playerState.NewVelocity;
            if (ValueComparator.IsEqual(playerCurrentVelocity.x, 0f) == false ||
                ValueComparator.IsEqual(playerCurrentVelocity.y, 0f) == false)
            {
                if (_isMovementCamActive == false)
                {
                    EnableMovementCam();
                }
                
                _idleEnableCounter = 0.0f;

                return; //If player is moving, simply return.
            }

            //Else apply movement of the object.
            float deltaTime = Time.fixedDeltaTime;
            _totalTime += deltaTime;
            if (IsIdleAnimEnabled(deltaTime))
            {
                if (_isMovementCamActive)
                {
                    EnableIdleCam();
                }
                
                //Start the animation only when the player's not moving for given amount of time.
                var translation = CalculateMovementVector();
                PerformTranslation(translation);

            }
        }
    }
}
//TODO - find a way to add scalable, readable field in Unity that defines the rect of idle cam movement.