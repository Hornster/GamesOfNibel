using System;
using Assets.Scripts.GUI.Gamemodes.CTF.SingleControls;

namespace Assets.Scripts.InspectorSerialization.Interfaces
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
