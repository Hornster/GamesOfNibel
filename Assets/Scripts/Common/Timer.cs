using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// Simple class for time measurement.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// The time this timer will wait before calling the provided callback.
        /// </summary>
        private float _maxAwaitTime;
        /// <summary>
        /// The time the timer has been waiting already.
        /// </summary>
        private float _currentAwaitTime;
        /// <summary>
        /// Called when the timer has reached provided max await time.
        /// </summary>
        private UnityAction _timeIsUp;
        /// <summary>
        /// Is the timer currently running.
        /// </summary>
        private bool _isRunning;

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
            _maxAwaitTime = maxAwaitTime;
            _timeIsUp += timeUpCallback;
        }
        /// <summary>
        /// Updates the time.
        /// </summary>
        public void Update()
        {
            if (!_isRunning) return;

            _currentAwaitTime += Time.deltaTime;

            if (_currentAwaitTime >= _maxAwaitTime)
            {
                IsTimeUp = true;
                _timeIsUp?.Invoke();
            }
        }
        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            _isRunning = true;
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
            _isRunning = false;
        }
        /// <summary>
        /// Resets and stops timer.
        /// </summary>
        public void ResetAndStop()
        {
            Stop();
            Reset();
        }
    }
}
