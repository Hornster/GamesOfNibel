using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float xInput{ get; set; }
    public float slopeDownAngle{ get; set; }
    public float slopeDownAngleOld{ get; set; }
    public float slopeSideAngle{ get; set; }
    public int facingDirection { get; set; } = 1;
    public bool isGrounded{ get; set; }
    public bool canJump{ get; set; }
    public bool isOnSlope{ get; set; }
    public bool isJumping{ get; set; }
    public bool canWalkOnSlope{ get; set; }
    public Vector2 newVelocity{ get; set; }
    public Vector2 newForce{ get; set; }
    public Vector2 colliderSize{ get; set; }
    public Vector2 slopeNormalPerp{ get; set; }
    public bool CanDoubleJump { get; set; }

    /// <summary>
    /// Checks if one-use skills can be reset. If yes - calls resetting method.
    /// </summary>
    public void ChkSkillResetPossible()
    {
        if (isGrounded || canWalkOnSlope)
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
    }
}