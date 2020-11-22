using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
{
    public interface ISceneData
    {
        List<GameObject> Players { get; }
        List<GameObject> Spawners { get; }
    }
}
