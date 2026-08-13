using UnityEngine;

namespace Player.Utilities
{
    public static class CurveSampler
    {
        // SINE
        public static float InSine(float x) => 1f - Mathf.Cos((x * Mathf.PI) / 2f);
        public static float OutSine(float x) => Mathf.Sin((x * Mathf.PI) / 2f);
        public static float InOutSine(float x) => -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;

        // QUAD
        public static float InQuad(float x) => x * x;
        public static float OutQuad(float x) => 1f - (1f - x) * (1f - x);
        public static float InOutQuad(float x) => x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;

        // CUBIC
        public static float InCubic(float x) => x * x * x;
        public static float OutCubic(float x) => 1f - Mathf.Pow(1f - x, 3f);
        public static float InOutCubic(float x) => x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;

        // QUART
        public static float InQuart(float x) => x * x * x * x;
        public static float OutQuart(float x) => 1f - Mathf.Pow(1f - x, 4f);
        public static float InOutQuart(float x) => x < 0.5f ? 8f * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;

        // EXPO
        public static float InExpo(float x) => x == 0 ? 0 : Mathf.Pow(2f, 10f * x - 10f);
        public static float OutExpo(float x) => x == 1 ? 1 : 1f - Mathf.Pow(2f, -10f * x);
        public static float InOutExpo(float x) => x == 0 ? 0 : x == 1 ? 1 : x < 0.5f ? Mathf.Pow(2f, 20f * x - 10f) / 2f : (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f;

        // BACK (The "Overshoot" curves)
        private const float c1 = 1.70158f;
        private const float c3 = c1 + 1f;
        public static float InBack(float x) => c3 * x * x * x - c1 * x * x;
        public static float OutBack(float x) => 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);

        // BOUNCE
        public static float OutBounce(float x)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            if (x < 1 / d1) return n1 * x * x;
            else if (x < 2 / d1) return n1 * (x -= 1.5f / d1) * x + 0.75f;
            else if (x < 2.5 / d1) return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            else return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
        public static float InBounce(float x) => 1f - OutBounce(1f - x);
    }
}
