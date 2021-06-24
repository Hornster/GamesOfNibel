using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    public class ComponentBasesFactory : IBasesFactory
    {
        public List<GameObject> CreateSpawner(SpawnerGroupConfig spawnerGroupConfig)
        {
            throw new NotImplementedException();
        }
    }
    //TODO Change spawnerGroupConfig into spawnerConfig. Or convert BaseData into BaseDataMonoBehaviour so
    //TODO it can be created in the editor for debugging purposes.
}
