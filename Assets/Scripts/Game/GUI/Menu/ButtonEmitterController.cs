using UnityEngine;

namespace Assets.Scripts.Game.GUI.Menu
{
    /// <summary>
    /// Controls the emitters on the assigned button control.
    /// </summary>
    public class ButtonEmitterController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        /// <summary>
        /// Stops the particle system.
        /// </summary>
        public void DisableParticleSystem()
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        /// <summary>
        /// Starts the particle system.
        /// </summary>
        public void EnableParticleSystem()
        {
            _particleSystem.Play(true);
        }
    }
}
