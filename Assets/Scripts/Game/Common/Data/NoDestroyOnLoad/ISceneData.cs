using System.Collections.Generic;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// An interface that can be found by Unity's FindObjectBy method.
    /// Contains dynamically loaded data, that can be passed to currently loaded map.
    /// </summary>
    public class ISceneData : MonoBehaviour
    {
        public virtual Dictionary<Teams, List<GameObject>> Players { get; protected set; }
        public virtual Dictionary<Teams, List<GameObject>> Bases { get; protected set; }
    }
}
