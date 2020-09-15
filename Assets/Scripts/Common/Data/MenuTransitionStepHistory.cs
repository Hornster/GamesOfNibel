using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;

namespace Assets.Scripts.Common.Data
{
    public class MenuTransitionStepHistory
    {
        /// <summary>
        /// Where was the transition made from.
        /// </summary>
        public MenuType TransitionFrom { get; set; }
        /// <summary>
        /// What was the transition's target.
        /// </summary>
        public MenuType TransitionTo { get; set; }
    }
}
