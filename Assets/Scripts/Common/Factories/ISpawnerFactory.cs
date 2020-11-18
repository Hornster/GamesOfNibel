using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Factories
{
    /// <summary>
    /// Used to spawn spawners.
    /// </summary>
    public interface ISpawnerFactory
    {
        GameObject CreateSpawner(/*SpawnerData spawnerData*/);//TODO
    }
}
