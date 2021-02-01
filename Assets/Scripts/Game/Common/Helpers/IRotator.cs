using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Helpers
{
    /// <summary>
    /// Allows for rotation o the object.
    /// </summary>
    public interface IRotator
    {

        /// <summary>
        /// Rotates the object by provided quaternion.
        /// </summary>
        /// <param name="rotation">Rotation describing quaternion.</param>
        void Rotate(Quaternion rotation);
    }
}
