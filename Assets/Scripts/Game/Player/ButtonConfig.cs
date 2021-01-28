using UnityEngine;

namespace Assets.Scripts.Game.Player
{
    /// <summary>
    /// Singleton class that stores controls config.
    /// </summary>
    public class ButtonConfig
    {
        //Axes names
        public const string HorizontalAxisName = "Horizontal";
        public const string VerticalAxisName = "Vertical";

        //Button bindings
        public KeyCode JumpButton { get; set; } = KeyCode.Space;
        public KeyCode GlideButton { get; set; } = KeyCode.LeftShift;
        /// <summary>
        /// A special type of button reserved for special actions in
        /// given modes.
        /// </summary>
        public KeyCode ModeSpecialButton { get; set; } = KeyCode.X;

        public KeyCode HelpButton { get; set; } = KeyCode.H;
        public KeyCode LeaveButton { get; set; } = KeyCode.Escape;

        private static ButtonConfig _instance;

        public static ButtonConfig GetInstance()
        {
            return _instance ?? (_instance = new ButtonConfig());
        }

        private ButtonConfig()
        {

        }
    }
}
