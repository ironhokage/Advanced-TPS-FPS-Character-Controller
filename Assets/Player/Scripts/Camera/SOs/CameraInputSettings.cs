using UnityEngine;

namespace Player.PlayerSettings.Camera
{
    [CreateAssetMenu(fileName = "CameraInputSettings", menuName = "Scriptable Objects/CameraInputSettings")]
    public class CameraInputSettings : ScriptableObject
    {
        [Header("Mouse Sensitivity")]
        [Range(0.1f, 0.5f)] public float xSensibility = 0.25f;
        [Range(0.1f, 0.5f)] public float ySensibility = 0.25f;
        
        [Space(10)]
        [Header("Joystick Sensitivity")]
        [Range(0.1f, 0.5f)] public float xJoySensibility = 0.15f;
        [Range(0.1f, 0.5f)] public float yJoySensibility = 0.15f;
        
        [Space(10)]
        [Header("Inversion")]
        public bool invertY;
        public bool invertX;
        // In Unity, AnimationCurve is the equivalent of Godot's Curve
        public AnimationCurve smoothingCurve;
        public AnimationCurve smoothingJoystickCurve;
    
        public float curveReferenceDpi = 800.0f;
    }
}
