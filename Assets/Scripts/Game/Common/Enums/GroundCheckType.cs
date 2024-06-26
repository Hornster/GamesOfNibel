﻿namespace Assets.Scripts.Game.Common.Enums
{
    /// <summary>
    /// Possible collision detection schemes.
    /// </summary>
    public enum GroundCheckType
    {
        /// <summary>
        /// Default detection scheme. All things considered.
        /// </summary>
        Default,
        /// <summary>
        /// Special scheme where the player should not be able to collide
        /// with one-way dropdown platforms.
        /// </summary>
        JumpingOffPlatform
    }
}
