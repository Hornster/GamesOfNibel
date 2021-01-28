using System;
using Assets.Scripts.Game.Common.Factories;

namespace Assets.Scripts.Game.InspectorSerialization.Interfaces
{
    /// <summary>
    /// Adapter for ICharacterFactory interface to be visible in the inspector. 
    /// </summary>
    [Serializable]
    public class ICharacterFactoryContainer : IUnifiedContainer<ICharacterFactory>
    {
        public ICharacterFactory Interface
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
