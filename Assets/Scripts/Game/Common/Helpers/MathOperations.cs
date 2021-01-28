using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Helpers
{
    /// <summary>
    /// Non-standard math operations for use thorough the code.
    /// </summary>
    public class MathOperations
    {
        /// <summary>
        /// Converts the int value of given mask to its index in the editor.
        /// </summary>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static int ConvertLayerMaskValueToIndex(LayerMask layerMask)
        {
            int layerIndex = 0;
            int currentLayerValue = layerMask.value;
            
            while (currentLayerValue > 1)
            {
                currentLayerValue >>= 1;
                layerIndex++;
            }

            return layerIndex;
        }

        /// <summary>
        /// Converts the int value of given mask to its index in the editor.
        /// </summary>
        /// <param name="layerIndex">The index of the layer in Unity editor.</param>
        /// <returns></returns>
        public static int ConvertLayerIndexToLayerMaskValue(int layerIndex)
        {
            if (layerIndex > 1)
            {
                int layerValue = 1;
                int moveValue = layerIndex;

                layerIndex = layerValue << moveValue;
            } 

            return layerIndex;
        }
    }
}
