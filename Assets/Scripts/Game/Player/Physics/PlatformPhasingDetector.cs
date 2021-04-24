using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Physics
{
    /// <summary>
    /// Detects when player entity phases through the platform.
    /// </summary>
    [RequireComponent(typeof(Timer))]
    public class PlatformPhasingDetector : MonoBehaviour
    {
        /// <summary>
        /// What parts of terrain are considered platforms?
        /// </summary>
        [SerializeField] private LayerMask _whatIsPlatform;
        /// <summary>
        /// In seconds, amount of time after which the phasing state will be automatically disabled.
        /// </summary>
        [SerializeField] private float _platformCollisionTimeout = 0.2f;
        /// <summary>
        /// State of the player.
        /// </summary>
        [SerializeField] private PlayerState _playerState;
        /// <summary>
        /// Converted to index (used by Unity gameobjects) platform layer value.
        /// </summary>
        private int _whatIsPlatformLayerIndex;
        /// <summary>
        /// Measures the time since the collision stopped.
        /// </summary>
        private Timer _timer;
        private void Start()
        {
            _timer = GetComponent<Timer>();
            _whatIsPlatformLayerIndex = MathOperations.ConvertLayerMaskValueToIndex(_whatIsPlatform);
            _timer.MaxAwaitTime = _platformCollisionTimeout;
            _timer.RegisterTimeoutHandler(PassingTimeout);
        }
        /// <summary>
        /// If detected collider is of _whatIsPlatform layer, notifies the playerstate about phasing through the platform.
        /// </summary>
        /// <param name="collider"></param>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_whatIsPlatformLayerIndex == collider.gameObject.layer)
            {
                _playerState.IsPhasingThroughPlatform = true;
                _timer.Reset();
                _timer.StartTimer();
            }
        }
        /// <summary>
        /// If detected collider is of _whatIsPlatform layer, notifies the playerstate about phasing through the platform.
        /// </summary>
        /// <param name="collider"></param>
        private void OnTriggerStay2D(Collider2D collider)
        {
            if (_whatIsPlatformLayerIndex == collider.gameObject.layer)
            {
                if (_playerState.IsPhasingThroughPlatform)
                {
                    _timer.Reset();
                }
                else
                {
                    if (_timer.IsRunning)
                    {
                        _timer.ResetAndStop();
                    }
                    //Seems like something turned the phasing off. Prevent the timer
                    //from accidentally turning it off upon changing.
                }
            }
        }

        /// <summary>
        /// Called when the timer reaches the timeout value.
        /// </summary>
        private void PassingTimeout()
        {
            //Helps with case when the character keeps running upwards and not fully phased through other platform.
            //Such case might result in the flag staying true.
            _playerState.IsPhasingThroughPlatform = false;
            _timer.Stop();
        }
        
    }
}
