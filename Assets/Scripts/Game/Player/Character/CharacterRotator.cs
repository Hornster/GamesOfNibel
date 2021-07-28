using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Character
{
    /// <summary>
    /// Rotates the character.
    /// </summary>
    public class CharacterRotator : MonoBehaviour
    {
        /// <summary>
        /// Used to rotate the player.
        /// </summary>
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private PlayerState _playerState;

        /// <summary>
        /// Turns the character around - horizontal mirror.
        /// </summary>
        public void TurnCharacterHorizontally()
        {
            _playerState.facingDirection *= -1;
            _characterTransform.Rotate(0.0f, 180.0f, 0.0f);
        }
        /// <summary>
        /// Rotates the character towards the wall they hold. If they hold
        /// both walls at once - right one is chosen.
        /// </summary>
        /// <param name="isHoldingWallLeft">Is the character holding wall to their left?</param>
        /// <param name="isHoldingWallRight">Is the character holding wall to their right?</param>
        public void TurnCharacterToWall(bool isHoldingWallLeft, bool isHoldingWallRight)
        {
            if (isHoldingWallRight && _playerState.facingDirection != (int)FacingDirectionEnum.Right)
            {
                TurnCharacterHorizontally();
            }
            else if (isHoldingWallLeft && _playerState.facingDirection != (int)FacingDirectionEnum.Left)
            {
                TurnCharacterHorizontally();
            }
        }
    }
}
