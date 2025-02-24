using System;
using UnityEngine;

namespace MyUtils
{
    public class TimeCounterSecond
    {
        private bool _isPause;
        private float _duration;
        private float _currentSeconds;
        private Action _onComplete;
        private bool _isLoop;

        public float CurrentSeconds => _currentSeconds;
        public void Init(float duration, Action onComplete = null, bool isLoop = false)
        {
            _isPause = false;
            _duration = duration;
            _currentSeconds = _duration;
            _onComplete = onComplete;
            _isLoop = isLoop;
        }

        public void Execute()
        {
            if (_isPause)
            {
                return;
            }

            _currentSeconds -= Time.deltaTime;

            if (_currentSeconds > 0)
            {
                return;
            }

            _onComplete?.Invoke();

            if (!_isLoop)
            {
                return;
            }

            _currentSeconds = _duration;
        }

        public void Pause()
        {
            _isPause = true;
        }

        public void Resume()
        {
            _isPause = false;
        }

        // Phương thức reset lại thời gian
        public void Reset()
        {
            _currentSeconds = _duration;
            _isPause = false;
        }
    }
}
