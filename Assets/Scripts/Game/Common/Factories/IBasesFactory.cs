using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    /// <summary>
    /// Used to spawn spawners.
    /// </summary>
    public interface IBasesFactory
    {
        List<GameObject> CreateSpawner(SpawnerGroupConfig spawnerGroupConfig);
    }
}
