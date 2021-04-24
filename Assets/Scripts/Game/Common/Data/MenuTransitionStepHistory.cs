using Assets.Scripts.Game.Common.Enums;

namespace Assets.Scripts.Game.Common.Data
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
