using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// An interface that can be found by Unity's FindObjectBy method.
    /// Contains dynamically loaded data, that can be passed to currently loaded map.
    /// </summary>
    public class ISceneData : MonoBehaviour
    {
        public virtual List<GameObject> Players { get; protected set; }
        public virtual Dictionary<Teams, List<GameObject>> Spawners { get; protected set; }
    }
}
