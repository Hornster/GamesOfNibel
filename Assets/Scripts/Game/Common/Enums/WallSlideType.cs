namespace Assets.Scripts.Game.Common.Enums
{
    /// <summary>
    /// Defines types of influence that can the wall slide skill have on the player during gameplay.
    /// </summary>
    public enum WallSlideType
    {
        //IMPORTANT!!!!
        //Each new type should be of power of 2, i.e. 1, 2, 4, 8, 16...
        //Types can be merged with each other and each bit in the integer value is assigned to given type.
        /// <summary>
        /// The player's velocity caps at provided value.
        /// </summary>
        GravityDecrease = 1,
        /// <summary>
        /// The gravity is decreased for the player during the slide, making them accelerate at
        /// slower rate. Must stay at the end!
        /// </summary>
        MaxVelocityCap = 2
    }
}
