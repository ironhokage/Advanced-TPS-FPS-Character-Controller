using System;

namespace Player.Utilities.Input.RuntimeScripts
{
    [Serializable]
    public class FloatInputBuffer : InputBufferBase
    {
        public float InputValue { get; set; }
        public float BufferValue { get; set; }

        public float GetBufferedValue(float rawInput, float deltaTime)
        {
            if (rawInput > 0f)
            {
                InputValue = rawInput;
                BufferValue = InputValue;
                return BufferValue;
            }

            UpdateTimer(deltaTime);
            BufferValue = IsActive ? InputValue : rawInput;

            return BufferValue;
        }
    }
}
