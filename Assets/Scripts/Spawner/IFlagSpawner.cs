using Assets.Scripts.Common.Enums;

namespace Assets.Scripts.Spawner
{
    /// <summary>
    /// Defines what a regular spawner should have.
    /// </summary>
    internal interface IFlagSpawner
    {
        /// <summary>
        /// Spawns NEW entity characteristic for the spawner.
        /// </summary>
        /// <param name="flagIniData">Data concerning the yet-to-be spawned flag.</param>
        void SpawnEntity(FlagIniData flagIniData);
    }
}