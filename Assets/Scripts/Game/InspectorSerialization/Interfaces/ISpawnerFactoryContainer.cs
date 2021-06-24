using System;
using Assets.Scripts.Game.Common.Factories;

namespace Assets.Scripts.Game.InspectorSerialization.Interfaces
{

    /// <summary>
    /// Adapter for ISpawnerFactory interface to be visible in the inspector. 
    /// </summary>
    [Serializable]
    public class ISpawnerFactoryContainer : IUnifiedContainer<IBasesFactory>
    {
        public IBasesFactory Interface
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
