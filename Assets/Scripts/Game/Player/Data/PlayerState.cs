using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Player.Gravity;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Player.Data
{
    /// <summary>
    /// Contains all necessary info about the player's physics and movement.
    /// </summary>
    [RequireComponent(typeof(LocalGravityManager))]
    public class PlayerState : MonoBehaviour
    {
        //TODO Debug, remove later
        [SerializeField] private UnityEvent _doubleJumpReset;

        //TODO end debug

        public float xInput{ get; set; }
        public float yInput { get; set; }
        public float slopeDownAngle{ get; set; }
        public float slopeDownAngleOld{ get; set; }
        public float slopeSideAngle{ get; set; }
        /// <summary>
        /// The distance by which the character shall move towards the ground the next frame to
        /// stay consistently close to the ground.
        /// </summary>
        public float DistanceToGround { get; set; }
        public float MaxFallingVelocityY { get; set; }
        public int facingDirection { get; set; } = 1;
        public bool isGrounded{ get; set; }
        /// <summary>
        /// Set to true when the character is standing directly on the ground.
        /// Literally, touching it with their feet. When that happens - you can safely reset
        /// velocity y to 0.0f.
        /// </summary>
        public bool IsStandingOnGround { get; set; }
        public bool canJump{ get; set; }
        public bool isOnSlope{ get; set; }
        public bool isJumping{ get; set; }
        /// <summary>
        /// Set to true when player tries to jump. The truth value is held until the set memory time runs out
        /// (counted from pressing the jump button) or the player jumps.
        /// </summary>
        public bool IsJumpRequestRemembered { get; set; }
        /// <summary>
        /// Set to true when player stands on the ground. The truth value is held until the set memory time runs out
        /// (counted from walking off an edge).
        /// </summary>
        public bool IsStandingOnGroundRemembered { get; set; }
        /// <summary>
        /// Set to true when player touches climbable wall. The truth value is held until the set memory time runs out
        /// (counted from letting go the wall).
        /// </summary>
        public bool IsTouchingWallRemembered { get; set; }
        /// <summary>
        /// Is the player increasing their Y position (going up) by other means than walking up a slope? Like jumping, for example?
        /// </summary>
        public bool IsAscending { get; set; }
        /// <summary>
        /// Used to lift the player off the ground. Initial jump state, where the player is jumping
        /// but the collision with the ground may still be present. Can be set and reset only via proper methods since
        /// has a timeout timer.
        /// </summary>
        public bool IsBeginningJump { get; private set; }
        public bool canWalkOnSlope{ get; set; }
        /// <summary>
        /// Holds a climbable wall.
        /// </summary>
        public bool IsTouchingWall { get; set; }
        /// <summary>
        /// Holds a climbable wall and is sliding down it.
        /// </summary>
        public bool IsWallSliding { get; set; }
        public bool CanDoubleJump { get; set; }
        public bool IsGliding { get; set; }
        /// <summary>
        /// Set to true whenever player is ascending and has phased through a platform.
        /// </summary>
        public bool IsPhasingThroughPlatform { get; set; }
        /// <summary>
        /// If there are any restrictions towards Y velocity - this flag should be set to true.
        /// </summary>
        public bool IsFallingVelocityCapped { get; set; }
        /// <summary>
        /// New velocity that will be applied to the player.
        /// </summary>
        public Vector2 NewVelocity{ get; set; }
        public Vector2 ColliderSize{ get; set; }
        public Vector2 SlopeNormalPerp{ get; set; }
        /// <summary>
        /// The normal of the slope the character is standing on, read from horizontal slope raycast check.
        /// When no collision found - set to [0;0]
        /// </summary>
        public Vector2 SlopeHorizontalNormal { get; set; }

        public GlideStages GlideStage { get; set; }

        public LocalGravityManager LocalGravityManager { get; private set; }
        /// <summary>
        /// Time, in seconds, after which the jump request is forgotten.
        /// </summary>
        [Header("Config")] 
        [SerializeField] private float _jumpRequestMemoryTimeout = 0.1f;
        /// <summary>
        /// Time, in seconds, after which the fact that the player was touching the ground is forgotten.
        /// </summary>
        [SerializeField] private float _groundTouchMemoryTimeout = 0.1f;
        /// <summary>
        /// Time, in seconds, after which the fact that the player was touching the wall is forgotten.
        /// </summary>
        [SerializeField] private float _wallTouchMemoryTimeout = 0.1f;
        /// <summary>
        /// Time, in seconds, after which the isBeginningJump boolean will be reset.
        /// </summary>
        [SerializeField] private float _isBeginningJumpTimeout = 1f;
        /// <summary>
        /// Measures the time from last jump input from player.
        /// </summary>
        [Header("Required references")]
        [SerializeField] private Timer _jumpMemoryTimer;
        /// <summary>
        /// Measures the time since last ground, or wall, touching by the player.
        /// </summary>
        [SerializeField] private Timer _groundTouchMemoryTimer;
        /// <summary>
        /// Measures the time since the most recent wall holding by the player.
        /// </summary>
        [SerializeField] private Timer _wallTouchMemoryTimer;
        /// <summary>
        /// A safety switch for the IsBeginningJump flag. In tight spaces jumping can result in
        /// the character not letting off the ground.
        /// </summary>
        [SerializeField] private Timer _beginningJumpTimer;

        private void Awake()
        {
            LocalGravityManager = GetComponent<LocalGravityManager>();
            _jumpMemoryTimer.MaxAwaitTime = _jumpRequestMemoryTimeout;
            _jumpMemoryTimer.RegisterTimeoutHandler(TimeoutForJumpMemory);

            _groundTouchMemoryTimer.MaxAwaitTime = _groundTouchMemoryTimeout;
            _groundTouchMemoryTimer.RegisterTimeoutHandler(TimeoutForGroundStandingMemory);

            _wallTouchMemoryTimer.MaxAwaitTime = _wallTouchMemoryTimeout;
            _wallTouchMemoryTimer.RegisterTimeoutHandler(TimeoutForHoldingWallMemory);

            _beginningJumpTimer.MaxAwaitTime = _isBeginningJumpTimeout;
            _beginningJumpTimer.RegisterTimeoutHandler(TimeoutForIsBeginningJumpBoolean);
        }
        /// <summary>
        /// Registers the double jump timer which is called each time the jump memory timeout happens.
        /// </summary>
        public void RegisterDoubleJumpHandler(UnityAction doubleJumpHandler)
        {
            _jumpMemoryTimer.RegisterTimeoutHandler(doubleJumpHandler);
        }
        /// <summary>
        /// Called when character is standing completely on the ground.
        /// </summary>
        public void CharacterFirmlyTouchedGround()
        {
            IsStandingOnGround = true;
            IsStandingOnGroundRemembered = true;
            _groundTouchMemoryTimer.Stop();
        }
        /// <summary>
        /// Called when the character is touching a climbable wall.
        /// </summary>
        public void CharacterTouchedTheWall()
        {
            IsTouchingWall = true;
            IsTouchingWallRemembered = true;
            _wallTouchMemoryTimer.Stop();
        }
        /// <summary>
        /// Should be called whenever the character stops touching the ground.
        /// </summary>
        public void CharacterStoppedTouchingGround()
        {
            isGrounded = false;
            IsStandingOnGround = false;
        }
        /// <summary>
        /// Checks if one-use skills can be reset. If yes - calls resetting method.
        /// </summary>
        public void ChkSkillResetPossible()
        {
            if (isGrounded || IsTouchingWall)
            {
                ResetOneUseSkills();
            }
        
        }

        public void PlayerWallJumps()
        {
            isJumping = true;
            IsAscending = true;
            IsTouchingWallRemembered = false;
            IsJumpRequestRemembered = false;

            _jumpMemoryTimer.Stop();//We jumped already, no need to jump twice.
            _wallTouchMemoryTimer.Stop();
        }
        /// <summary>
        /// Called when the player jumps from the walkable ground.
        /// </summary>
        public void PlayerJumps()
        {
            canJump = false;
            isJumping = true;
            IsAscending = true;
            IsStandingOnGroundRemembered = false;
            IsJumpRequestRemembered = false;
        
            _jumpMemoryTimer.Stop();//We jumped already, no need to jump twice.
            _groundTouchMemoryTimer.Stop();
            _wallTouchMemoryTimer.Stop();
            SetIsBeginningJump();
        }
        /// <summary>
        /// Called whenever the player reaches the max height of their jump.
        /// </summary>
        public void MaxJumpHeightReached()
        {
            isJumping = false;
            IsAscending = false;
            IsPhasingThroughPlatform = false;
        }
        /// <summary>
        /// Resets all basic movement one-time skills.
        /// </summary>
        public void ResetOneUseSkills()
        {
            CanDoubleJump = true;

            NotifyDebugWatchers();
        }
        /// <summary>
        /// Called when the player walks off the ground (ground no longer detected but not because of jumping).
        /// </summary>
        public void PlayerWalkedOffGround()
        {
            _groundTouchMemoryTimer.Reset();
            _groundTouchMemoryTimer.StartTimer();
        }
        /// <summary>
        /// Called when the player lets go off a wall (no longer detected but not because of jumping).
        /// </summary>
        public void PlayerLetGoWall()
        {
            _wallTouchMemoryTimer.Reset();
            _wallTouchMemoryTimer.StartTimer();
        }
    
        /// <summary>
        /// Called when player presses the jump control. This does not ensure jump, merely remembers the request
        /// for time set in the config. Used both for wall and regular jumps.
        /// </summary>
        public void PlayerRequestedJump()
        {
            IsJumpRequestRemembered = true;
            _jumpMemoryTimer.Reset();
            _jumpMemoryTimer.StartTimer();
        }

        public void ResetState()
        {
            xInput = 0;
            yInput = 0;
            IsJumpRequestRemembered = false;
            IsPhasingThroughPlatform = false;
            IsStandingOnGroundRemembered = false;
            IsTouchingWallRemembered = false;
            IsGliding = false;
            ResetOneUseSkills();
            NewVelocity = new Vector2(0, 0);
            GlideStage = GlideStages.GlideStop;
        }
        /// <summary>
        /// Resets the IsBeginningJump boolean and its timeout timer.
        /// </summary>
        public void ResetIsBeginningJump()
        {
            IsBeginningJump = false;
            TimeoutForIsBeginningJumpBoolean();
        }
        /// <summary>
        /// Sets the IsBeginningJump boolean and launches its timeout timer.
        /// </summary>
        public void SetIsBeginningJump()
        {
            IsBeginningJump = true;
            _beginningJumpTimer.StartTimer();
        }

        /// <summary>
        /// Called by jump timeout memory timer when the time after pressing the jump button has passed.
        /// </summary>
        private void TimeoutForJumpMemory()
        {
            IsJumpRequestRemembered = false;
            _jumpMemoryTimer.Stop();
        }
        /// <summary>
        /// Called by wall holding timeout memory timer when the time after letting go the wall has passed.
        /// </summary>
        private void TimeoutForHoldingWallMemory()
        {
            IsTouchingWallRemembered = false;
            _wallTouchMemoryTimer.Stop();
        }
        /// <summary>
        /// Called by wall holding timeout memory timer when the time walking off the ground has passed.
        /// </summary>
        private void TimeoutForGroundStandingMemory()
        {
            IsStandingOnGroundRemembered = false;
            _groundTouchMemoryTimer.Stop();
        }
        /// <summary>
        /// Called by beginning jump boolean timer when the time runs out and the boolean is still set to true.
        /// </summary>
        private void TimeoutForIsBeginningJumpBoolean()
        {
            IsBeginningJump = false;
            _beginningJumpTimer.ResetAndStop();
            Debug.Log("IsBeginningJump reset!");
        }
        //todo DEBUG
        private void NotifyDebugWatchers()
        {
            if (CanDoubleJump)
            {
                _doubleJumpReset?.Invoke();
            }
        }
    }
}