using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Helpers
{
    /// <summary>
    /// Used to scale objects.
    /// </summary>
    public interface IScaler
    {
        void ChangeScale(Vector3 newScale);
    }
}
