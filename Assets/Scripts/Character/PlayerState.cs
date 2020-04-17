using UnityEngine;

public class PlayerState
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
}