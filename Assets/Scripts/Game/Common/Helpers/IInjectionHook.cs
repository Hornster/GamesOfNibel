using UnityEngine;

namespace Assets.Scripts.Game.Common.Helpers
{
    /// <summary>
    /// Used to inject component dependencies to other objects, dynamically.
    /// </summary>
    public interface IInjectionHook
    {
        void InjectReferences(GameObject source);
    }
}
