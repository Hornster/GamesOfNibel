using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Factories;

namespace Assets.Scripts.InspectorSerialization.Interfaces
{

    /// <summary>
    /// Adapter for ISpawnerFactory interface to be visible in the inspector. 
    /// </summary>
    [Serializable]
    public class ISpawnerFactoryContainer : IUnifiedContainer<ISpawnerFactory>
    {
        public ISpawnerFactory Interface
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
