﻿using Assets.Scripts.Common;
using Assets.Scripts.Player.Gravity;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains all necessary info about the player's physics and movement.
/// </summary>
[RequireComponent(typeof(GravityManager))]
public class PlayerState : MonoBehaviour
{
    //TODO Debug, remove later
    [SerializeField] private UnityEvent _doubleJumpReset;
    //TODO end debug

    public float xInput{ get; set; }
    public float slopeDownAngle{ get; set; }
    public float slopeDownAngleOld{ get; set; }
    public float slopeSideAngle{ get; set; }
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
    /// If there are any restrictions towards Y velocity - this flag should be set to true.
    /// </summary>
    public bool IsFallngVelocityCapped { get; set; }
    public Vector2 newVelocity{ get; set; }
    public Vector2 colliderSize{ get; set; }
    public Vector2 slopeNormalPerp{ get; set; }

    public GravityManager GravityManager { get; private set; }

    private void Awake()
    {
        GravityManager = GetComponent<GravityManager>();
    }
    /// <summary>
    /// Should be called whenever the character stops touching the ground.
    /// </summary>
    public void CharacterStoppedTouchingTheGround()
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
    /// <summary>
    /// Resets all basic movement one-time skills.
    /// </summary>
    public void ResetOneUseSkills()
    {
        CanDoubleJump = true;

        NotifyDebugWatchers();
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