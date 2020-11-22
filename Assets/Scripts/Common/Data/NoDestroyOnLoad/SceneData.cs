using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Common.Factories;
using Assets.Scripts.InspectorSerialization.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Contains all necessary data for a scene to be loaded.
    /// </summary>
    public class SceneData : MonoBehaviour, ISceneData
    {
        
        /// <summary>
        /// List of all created players.
        /// </summary>
        public List<GameObject> Players { get; set; } = new List<GameObject>();
        /// <summary>
        /// List of all created spawners.
        /// </summary>
        public List<GameObject> Spawners { get; set; } = new List<GameObject>();

        
        /// <summary>
        /// Marks this object to be destroyed.
        /// </summary>
        public void DestroyData()
        {
            Destroy(this);
        }
    }
}
