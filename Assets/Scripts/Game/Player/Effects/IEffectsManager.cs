namespace Assets.Scripts.Game.Player.Effects
{
    /// <summary>
    /// Determines what should an effect manager have.
    /// </summary>
    public interface IEffectsManager
    {
        /// <summary>
        /// Adds a physical effect to the manager.
        /// </summary>
        /// <param name="effect"></param>
        void AddPhysicalEffect(IPlayerPhysicalEffect effect);
    }
}
