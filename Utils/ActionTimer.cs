using System;

namespace Utils
{
    public class ActionTimer
    {
        private readonly Action _callback;
        private readonly float _actionRate;

        private float _elapsedTime;

        public ActionTimer(float actionRate, System.Action callback)
        {
            _actionRate = actionRate;
            _callback = callback;
        }

        public void Elapse(float elapsedTime)
        {
            _elapsedTime += elapsedTime;

            if (_elapsedTime >= _actionRate)
            {
                _callback();
                _elapsedTime -= _actionRate;
            }
        }

        public void Reset()
        {
            _elapsedTime = 0f;
        }
    }
}
