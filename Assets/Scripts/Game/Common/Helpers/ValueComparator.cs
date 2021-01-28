using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Helpers
{
    /// <summary>
    /// Used to compare non-standard or troublesome values.
    /// </summary>
   public static class ValueComparator
    {
        public static bool IsEqual(float left, float right, float precision = 0.0001f)
        {
            float difference = Mathf.Abs(left - right);

            return difference <= precision;
        }
    }
}
