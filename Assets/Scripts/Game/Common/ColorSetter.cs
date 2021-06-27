using System;
using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Sets the color of the sprite assigned to it.
    /// </summary>
    public class ColorSetter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] _spriteRenderers;

        public bool OverrideAlpha = false;

        public float AlphaOverrideValue = 0f;

        /// <summary>
        /// Sets the color of the sprite accordingly to the team. Changes the team as well.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            if (OverrideAlpha)
            {
                color.a = Mathf.Clamp01(AlphaOverrideValue);
            }
            foreach(var spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.color = color;
            }
        }
    }
}
