using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.Physics
{
    /// <summary>
    /// Defines what should a simple raycaster do.
    /// </summary>
    public interface IRaycaster2D
    {
        /// <summary>
        /// Casts a single ray. Returns the cast result.
        /// </summary>
        /// <param name="direction">Direction in which the ray shall be cast.</param>
        /// <param name="whatCollides">What layers does the ray shall collide with.</param>
        /// <returns></returns>
        ref RaycastHit2D CastRay(Vector2 direction, LayerMask whatCollides);
    }
}
