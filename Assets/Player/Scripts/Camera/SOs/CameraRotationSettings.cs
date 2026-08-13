using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Camera.SOs
{
    [CreateAssetMenu(fileName = "CameraRotationSettings", menuName = "Scriptable Objects/CameraRotationSettings")]
    public class CameraRotationSettings : ScriptableObject
    {
        [field: Header("FPS Y Axis Rotation Limits")]
        
        [field: Tooltip("Maximum up pitch in degrees.")]
        [Range(-90.0f, 0.0f)]
        [field: SerializeField] public float FPSMaxUpPitch { get; set; } = -65.0f;

        [field: Tooltip("Maximum down pitch in degrees.")]
        [Range(0.0f, 90.0f)]
        [field: SerializeField] public float FPSMaxDownPitch { get; set; } = 65.0f;
        
        [field: Space(5)]
        [field: Header("TPS Y Axis Rotation Limits")]
        [field: Tooltip("Maximum up pitch in degrees.")]
        [Range(-90.0f, 0.0f)]
        [field: SerializeField] public float TPSMaxUpPitch { get; set; } = -65.0f;

        [field: Tooltip("Maximum down pitch in degrees.")]
        [Range(0.0f, 90.0f)]
        [field: SerializeField] public float TPSMaxDownPitch { get; set; } = 65.0f;
        
        [field: Space(5)]
        [field: Header("Rotation Soft Dead Zone")]
        [field: Tooltip("The range in degrees where sensitivity/smoothing is adjusted for finer control.")]
        [Range(1.0f, 30.0f)]
        [field: SerializeField] public float SoftZoneDegrees { get; set; } = 20.0f;
        
        [field: Space(5)]
        [field: Header("Smoothing Factors")]
        [field: Tooltip("Smoothing factors for X and Y mouse movement.")]
        [field: SerializeField] public float SmoothingFactorX { get; set; } = 9.75f;
        [field: SerializeField] public float SmoothingFactorY { get; set; } = 7.75f; 
        
        [field: SerializeField] public float TargetPitch { get; set; } = 9.75f;
        
    }
}

