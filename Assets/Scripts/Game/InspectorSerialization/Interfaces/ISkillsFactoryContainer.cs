using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Factories;
using Assets.Scripts.Game.Player.Character.Skills.Factory;

namespace Assets.Scripts.Game.InspectorSerialization.Interfaces
{
    /// <summary>
    /// Adapter for ISkillsFactory interface to be visible in the inspector. 
    /// </summary>
    [Serializable]
    public class ISkillsFactoryContainer : IUnifiedContainer<ISkillsFactory>
    {
        public ISkillsFactory Interface
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
