using System;
using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Sets the color of the sprite assigned to it.
    /// </summary>
    public class ColorSetter : MonoBehaviour
    {
        public SpriteRenderer[] SpriteRenderers;

        [HideInInspector]
        public bool OverrideAlpha = false;
        [HideInInspector]
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
            foreach(var spriteRenderer in SpriteRenderers)
            {
                spriteRenderer.color = color;
            }
        }
    }
}
