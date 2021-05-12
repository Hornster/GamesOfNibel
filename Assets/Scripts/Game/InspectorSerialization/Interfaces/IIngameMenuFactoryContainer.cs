using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Factories;

namespace Assets.Scripts.Game.InspectorSerialization.Interfaces
{
    /// <summary>
    /// Adapter for IIngameFactory interface to be visible in the inspector
    /// </summary>
    [Serializable]
    public class IIngameMenuFactoryContainer : IUnifiedContainer<IGameGUIFactory>
    {
        public IGameGUIFactory Interface
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
