using System;

namespace Utils
{
    public class CooldownTimer
    {
        private float _remainingCooldownTime;

        public void Elapse(float elapsedTime)
        {
            if (!IsReady())
            {
                _remainingCooldownTime = Math.Clamp(_remainingCooldownTime - elapsedTime, 0f, _remainingCooldownTime);
            }
        }

        public void Start(float cooldownTime)
        {
            _remainingCooldownTime = cooldownTime;
        }

        public bool IsReady()
        {
            return _remainingCooldownTime == 0f;
        }
    }
}
