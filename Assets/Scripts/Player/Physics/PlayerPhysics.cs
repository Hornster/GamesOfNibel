using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player.Character;
using UnityEngine;

namespace Assets.Scripts.Player.Physics
{
    public class PlayerPhysics : MonoBehaviour
    {
        [Header("Config values")]
        [SerializeField]
        private float _verticalSlopeCheckDistance;
        [SerializeField]
        private float _horizontalSlopeCheckDistance;
        [SerializeField]
        private float maxSlopeAngle;
        [SerializeField]
        private float groundCheckRadius;
        [SerializeField]
        private float wallCheckDistance;
        /// <summary>
        /// Max angle of the wall towards horizontal ground that the player can climb on (or perform a wall jump).
        /// </summary>
        [SerializeField]
        private float _maxClimbableAngle;
        /// <summary>
        /// Values above that angle indicate that the ground stopped being a slope and began being regular wall.
        /// </summary>
        [SerializeField]
        private float _minimalWallAngle;
        /// <summary>
        /// Offset from the ground - minimal distance the character will try to keep from the ground.
        /// </summary>
        [SerializeField]
        private float _skinWidth = 0.01f;
        /// <summary>
        /// How close the character has to be to the ground to be considered standing on it.
        /// </summary>
        [SerializeField] private float _closeToGroundThreshold = 0.1f;

        [SerializeField] private Vector2 _closeGroundCheckSize;

        /// <summary>
        /// Defines where will the collision and slope checking rays be coming from.
        /// </summary>
        [Header("Required references")]
        [SerializeField]
        private Transform groundCheck;
        /// <summary>
        /// Start position for additional ray that checks if the character touches a long enough part of wall for
        /// climbing/wall jumping.
        /// </summary>
        [SerializeField] private Transform wallCheck;



        //Component references
        [SerializeField]
        private PhysicsMaterial2D noFriction;
        [SerializeField]
        private PhysicsMaterial2D fullFritcion;

        [SerializeField] private CharacterRotator _characterRotator;
        [SerializeField] private CollisionMasksManager _collisionMaskManager;
        [SerializeField] private RaycastController _groundRaysController;
        [SerializeField] private RaycastController _wallRaysController;
        private Rigidbody2D rb;
        private PlayerState _playerState;
        /// <summary>
        /// Set to true when there's an unclimbable slope detected to the front of the character.
        /// </summary>
        private bool _unclimbableSlopeOnFront;
        /// <summary>
        /// Set to true when there's an unclimbable slope detected to the back of the character.
        /// </summary>
        private bool _unclimbableSlopeOnBack;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerState = GetComponent<PlayerState>();
        }



        public void CheckCollisions()
        {
            _unclimbableSlopeOnFront = false;
            _unclimbableSlopeOnBack = false;

            CheckGround();      //Check if the player is touching the ground or, simply, is close enough to it.
            //MoveCloserToGround();//Check distance to the ground to move the player as close to the ground as possible.
            SlopeCheck();
            WallCheck();
            ChkEdgeCases();
        }

        private Vector2 GetCheckPos()
        {
            float scaledColliderSize = (_playerState.ColliderSize.y * transform.localScale.y);
            return transform.position - new Vector3(0, scaledColliderSize / 2, 0);
        }
        /// <summary>
        /// Moves the character closer to he ground.
        /// </summary>
        private void MoveCloserToGround()
        {
            var checkPos = GetCheckPos();
            var groundHit = _groundRaysController.CastAllRays(Vector2.down, _collisionMaskManager.WhatIsGround);//Physics2D.Raycast(checkPos, Vector2.down, groundCheckRadius, _collisionMaskManager.WhatIsGround);

            if (_playerState.isGrounded == false)
            {
                _playerState.CharacterStoppedTouchingGround();
            }

            if (groundHit)
            {
                _playerState.isGrounded = true;
                
                if (groundHit.distance <= _closeToGroundThreshold && _playerState.IsBeginningJump == false)
                {
                    //We are close enough to the ground to walk on it and are not jumping already
                    _playerState.CharacterFirmlyTouchedGround();
                }

                var whatIsPlatformLayer = MathOperations.ConvertLayerMaskValueToIndex(_collisionMaskManager.WhatIsPlatform);
                if (_playerState.IsPhasingThroughPlatform 
                    && groundHit.collider.gameObject.layer == whatIsPlatformLayer
                    && _playerState.IsAscending)
                {
                    //If we hit a platform while ascending, we shouldn't be pulled towards it to be able to fly through it.
                    _playerState.DistanceToGround = 0.0f;
                    _playerState.CharacterStoppedTouchingGround();
                }
            }
            else
            {
                _playerState.CharacterStoppedTouchingGround();
                _playerState.DistanceToGround = 0.0f;
            }

            if (_playerState.IsStandingOnGround == false || _playerState.isJumping || _playerState.canWalkOnSlope == false)
            {
                _playerState.DistanceToGround = 0.0f;
                return; // No need to check anything if the player is NOT standing on the ground.
                        // What would they be moved towards?
            }

            //Player is above the ground and we can move them closer towards it.
            _playerState.DistanceToGround = groundHit.distance - _skinWidth;
        }
        private void CheckGround()
        {
            MoveCloserToGround();//Check distance to the ground to move the player as close to the ground as possible.
            if (_playerState.isGrounded == false)
            {
                //As soon as we let go the ground, we need to be prepared for touching it.
                //Otherwise, jumping on slope under high angle while running towards that slope
                //would end up in us slide upwards up to the moment when this flag changes back to false.
                //Which by default happens upon velocity.y reaching 0 from positive value.
                _playerState.IsBeginningJump = false;
            }

            if (rb.velocity.y <= 0.0f)
            {
                _playerState.MaxJumpHeightReached();
            }
            if (_playerState.isGrounded && !_playerState.isJumping && _playerState.slopeDownAngle <= maxSlopeAngle)
            {
                _playerState.canJump = true;
            }
            else
            {
                _playerState.canJump = false;
            }

        }

        private void SlopeCheck()
        {
            var checkPos = GetCheckPos();

            SlopeCheckHorizontal(checkPos);
            SlopeCheckVertical(checkPos);

        }
        private void SlopeCheckHorizontal(Vector2 checkPos)
        {
            var slopeHitFront = Physics2D.Raycast(checkPos, transform.right, _horizontalSlopeCheckDistance, _collisionMaskManager.WhatIsGround);
            var slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, _horizontalSlopeCheckDistance, _collisionMaskManager.WhatIsGround);

            if (slopeHitBack)
            {
                _unclimbableSlopeOnBack = true;
            }

            if (slopeHitFront)
            {
                _unclimbableSlopeOnFront = true;
                _playerState.isOnSlope = true;
                _playerState.slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
                _playerState.SlopeHorizontalNormal = slopeHitFront.normal;
            }
            else if (slopeHitBack)
            {
                _playerState.isOnSlope = true;
                _playerState.slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
                _playerState.SlopeHorizontalNormal = slopeHitBack.normal;
            }
            else
            {
                _playerState.slopeSideAngle = 0.0f;
                _playerState.isOnSlope = false;
                _playerState.SlopeHorizontalNormal = Vector2.zero;
            }

        }
        /// <summary>
        /// Checks for the slope right beneath the player.
        /// </summary>
        /// <param name="checkPos">Position which the testing ray will be cast from.</param>
        private void SlopeCheckVertical(Vector2 checkPos)
        {
            var hit = Physics2D.Raycast(checkPos, Vector2.down, _verticalSlopeCheckDistance, _collisionMaskManager.WhatIsGround);
            Debug.DrawRay(checkPos, Vector2.down * _verticalSlopeCheckDistance, Color.black);
            if (hit)
            {
                _playerState.SlopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

                _playerState.slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (ValueComparator.IsEqual(_playerState.slopeDownAngle, _playerState.slopeDownAngleOld) == false)
                {
                    _playerState.isOnSlope = true;
                }

                _playerState.slopeDownAngleOld = _playerState.slopeDownAngle;

                Debug.DrawRay(hit.point, _playerState.SlopeNormalPerp, Color.red);
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            }

            if (_playerState.slopeDownAngle > maxSlopeAngle || _playerState.slopeSideAngle > maxSlopeAngle)
            {
                _playerState.canWalkOnSlope = false;
            }
            else
            {
                _playerState.canWalkOnSlope = true;
                if (_playerState.IsStandingOnGround == false && rb.velocity.y <= 0.0f
                || _playerState.IsStandingOnGround && _playerState.IsBeginningJump == false)
                {
                    _playerState.MaxJumpHeightReached();
                }
            }

            if (_playerState.isOnSlope && ValueComparator.IsEqual(_playerState.xInput, 0.0f) && _playerState.canWalkOnSlope)
            {
                rb.sharedMaterial = fullFritcion;
            }
            else
            {
                rb.sharedMaterial = noFriction;
            }
        }
        /// <summary>
        /// Checks if the character is holding a wall.
        /// </summary>
        private void WallCheck()
        {
            var direction = Vector2.right * _playerState.facingDirection;
            var raycastResult = _wallRaysController.CastAllRays(direction, _collisionMaskManager.WallCollisionLayers);
            _playerState.IsTouchingWall = raycastResult;

            if (raycastResult)
            {
                //Wall is hit. Get the angle of the wall towards the ground.
                var angle = Vector2.Angle(raycastResult.normal, Vector2.up);

                _playerState.IsTouchingWall = angle <= _maxClimbableAngle;
            }
            
            if (_playerState.IsTouchingWall && _playerState.isGrounded == false && rb.velocity.y < 0)
            {
                _playerState.IsWallSliding = true;
            }
            else
            {
                _playerState.IsWallSliding = false;
            }
        }
        /// <summary>
        /// Checks for these pesky edge cases that happen rarely but when they do they break the gameplay, most likely.
        /// </summary>
        private void ChkEdgeCases()
        {
            //Check if the character is hanging in between two unclimbable slopes that are too short for a wall jump.
            if (_playerState.isGrounded && _unclimbableSlopeOnBack && _unclimbableSlopeOnFront)
            {
                _playerState.canJump = true;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            var wallLineDest = new Vector3(wallCheck.position.x + transform.right.x * wallCheckDistance, wallCheck.position.y, wallCheck.position.z);
            Gizmos.DrawLine(wallCheck.position, wallLineDest);

            Vector2 checkPos = transform.position;
            var slopeCheckDest = checkPos + (Vector2)transform.right * _horizontalSlopeCheckDistance;
            Gizmos.DrawLine(checkPos, slopeCheckDest);

        }

    }
}
//TODO: Add 2 more rays to bottom and one more to wall detection.
//TODO: Each ray would have it's own game object. These would be in hierarchy as childs of a controller
//TODO:The controller would seek out the rays with GetComponentsInChildren
//TODO: Wall jump can be done basing on the timer as well. Game remembers that the character was holding the wall
//TODO: and measures time, for example allowing the player to jump up to 0.2 seconds after letting go the wall.