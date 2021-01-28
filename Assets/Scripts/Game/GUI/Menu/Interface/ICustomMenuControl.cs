namespace Assets.Scripts.Game.GUI.Menu.Interface
{
    /// <summary>
    /// Defines base for every single custom UI control.
    /// </summary>
    public interface ICustomMenuControl
    {
        /// <summary>
        /// The ID of this control. Ought to be unique in the controller that this control belongs to.
        /// </summary>
        int ControlId { get; set; }

        /// <summary>
        /// Deselects the control.
        /// </summary>
        void DeselectControl();

        /// <summary>
        /// Selects the control.
        /// </summary>
        void SelectControl();

        /// <summary>
        /// Called upon pressing/using the control.
        /// </summary>
        void ControlPressed();
        /// <summary>
        /// Called when the pointer left the control's area.
        /// </summary>
        void PointerLeftControl();
    }
}
