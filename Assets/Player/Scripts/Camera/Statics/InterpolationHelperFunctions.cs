using UnityEngine;

namespace Player.Camera.Statics
{
    public static class InterpolationHelperFunctions 
    {
        public static float ExpDecayValue(float value, float deltaTime) =>
            1f - Mathf.Exp(-deltaTime * value);

        public static float ValueInterpolation(float startValue, float endValue, float decayValue, float deltaTime) => 
            Mathf.Lerp(startValue, endValue, ExpDecayValue(decayValue,  deltaTime));
    }
}
