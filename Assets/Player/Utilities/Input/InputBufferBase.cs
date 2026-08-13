using UnityEngine;

namespace Player.Utilities.Input
{
    [System.Serializable]
    public abstract class InputBufferBase
    {
        public float holdTime = 0.25f;
        public float timerValue ;
        public bool IsActive { get; private set; }

        public void UpdateTimer(float deltaTime)
        {
            timerValue -= deltaTime;
            timerValue = Mathf.Clamp(timerValue, 0f, holdTime);
            
            IsActive = !(timerValue <= 0f);
        }
        
        public void StartBuffer()
        {
            timerValue = holdTime;
            IsActive = true;
        }

        public void StopBuffer()
        {
            IsActive = false;
            timerValue = 0f;
        }
    }
}
