using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Factories;

namespace Assets.Scripts.Game.InspectorSerialization.Interfaces
{
    /// <summary>
    /// Adapter for IGameModeControllerFactory interface to be visible in the inspector
    /// </summary>
    [Serializable]
    public class IGameModeControllerFactoryContainer :IUnifiedContainer<IGameModeControllerFactory>
    {
        public IGameModeControllerFactory Interface
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
