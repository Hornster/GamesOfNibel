using System;
using Assets.Scripts.Game.GUI.Gamemodes.CTF.SingleControls;

namespace Assets.Scripts.Game.InspectorSerialization.Interfaces
{
    [Serializable]
    public class ITextControlContainer : IUnifiedContainer<IGuiTextControl>
    {
        public IGuiTextControl Interface
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
