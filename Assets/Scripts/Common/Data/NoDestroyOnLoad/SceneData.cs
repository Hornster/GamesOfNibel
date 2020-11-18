using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Contains all necessary data for a scene to be loaded.
    /// </summary>
    public class SceneData : MonoBehaviour, ISceneData
    {
        private List<GameObject> _players;
        private List<GameObject> _spawners;
        public GameObject[] GetPlayers()
        {
            return _players.ToArray();
        }

        public GameObject[] GetSpawns()
        {
            return _spawners.ToArray();
        }
    }
}
