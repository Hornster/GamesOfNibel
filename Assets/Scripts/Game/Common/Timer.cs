using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Simple class for time measurement.
    /// </summary>
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// The time this timer will wait before calling the provided callback.
        /// </summary>
        public float MaxAwaitTime { get; set; } = 1.0f;
        /// <summary>
        /// The time the timer has been waiting already.
        /// </summary>
        private float _currentAwaitTime;
        /// <summary>
        /// Called when the timer has reached provided max await time.
        /// </summary>
        private UnityAction _timeRanOutHandler;
        /// <summary>
        /// Called every frame when the timer is running.
        /// </summary>
        private UnityAction<float> _periodicCallHandler;
        /// <summary>
        /// Is the timer currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Set to true when the time is up.
        /// </summary>
        public bool IsTimeUp { get; private set; } = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxAwaitTime">The time which the timer will await until calling the callback, in seconds.</param>
        /// <param name="timeUpCallback">Callback called when the time runs out (reaches maxAwaitTime). The callback is added,
        /// so providing multiple callbacks in the param will make the timer call all the provided callbacks.</param>
        public Timer(float maxAwaitTime, UnityAction timeUpCallback)
        {
            MaxAwaitTime = maxAwaitTime;
            _timeRanOutHandler += timeUpCallback;
        }
        /// <summary>
        /// Updates the time.
        /// </summary>
        private void Update()
        {
            if (!IsRunning) return;

            _currentAwaitTime += Time.deltaTime;

            _periodicCallHandler?.Invoke(_currentAwaitTime);

            if (_currentAwaitTime >= MaxAwaitTime)
            {
                IsTimeUp = true;
                _timeRanOutHandler?.Invoke();
            }
        }
        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer()
        {
            IsRunning = true;
        }
        /// <summary>
        /// Resets the timer to 0. Does not change the running state.
        /// </summary>
        public void Reset()
        {
            _currentAwaitTime = 0.0f;
            IsTimeUp = false;
        }
        /// <summary>
        /// Stops the timer. Does not reset it.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }
        /// <summary>
        /// Resets and stops timer.
        /// </summary>
        public void ResetAndStop()
        {
            Stop();
            Reset();
        }
        /// <summary>
        /// The registered event handlers will be called when the timer reaches the max set value (time runs out).
        /// </summary>
        /// <param name="timeoutHandler"></param>
        public void RegisterTimeoutHandler(UnityAction timeoutHandler)
        {
            _timeRanOutHandler += timeoutHandler;
        }
        
        /// <summary>
        /// The registered event handlers will be called every frame as long as the timer is running.
        /// </summary>
        /// <param name="periodicHandler">Handler that accepts one argument - current time of the timer.</param>
        public void RegisterPeriodicHandler(UnityAction<float> periodicHandler)
        {
            _periodicCallHandler += periodicHandler;
        }

        /// <summary>
        /// Clears all handlers connected to the timeout event.
        /// </summary>
        public void ClearTimeoutHandlers()
        {
            _timeRanOutHandler = null;
        }
        /// <summary>
        /// Clears all handlers connected to the event.
        /// </summary>
        public void ClearPeriodicHandler()
        {
            _periodicCallHandler = null;
        }
    }
}
