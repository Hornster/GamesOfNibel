namespace Assets.Scripts.Common.Data.ScriptableObjects.MapSelection
{
    /// <summary>
    /// Used to retrieve Json-formatted info about the mod.
    /// </summary>
    public interface IModJsonInfoFile
    {
        /// <summary>
        /// Returns Json representation of the mod data object.
        /// </summary>
        /// <returns></returns>
        string GetJsonString();
    }
}