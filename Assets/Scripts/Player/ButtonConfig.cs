using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ButtonConfig
    {
        //Axes names
        public const string HorizontalAxisName = "Horizontal";
        public const string VerticalAxisName = "Vertical";

        //Button bindings
        public KeyCode JumpButton { get; set; } = KeyCode.Space;

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
