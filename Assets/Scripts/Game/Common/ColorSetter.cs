using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// Sets the color of the sprite assigned to it.
    /// </summary>
    public class ColorSetter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Sets the color of the sprite accordingly to the team. Changes the team as well.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}
