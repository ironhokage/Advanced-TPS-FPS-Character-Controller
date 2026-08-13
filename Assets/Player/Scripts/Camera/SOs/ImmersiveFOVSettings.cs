using UnityEngine;

namespace Player.PlayerSettings.Camera
{
    [CreateAssetMenu(fileName = "ImmersiveFOVSettings", menuName = "Scriptable Objects/ImmersiveFOVSettings")]
    public class ImmersiveFOVSettings : ScriptableObject
    {
        [Header("Interpolation Settings")]
        [Tooltip("How quickly the camera position follows the target offset.")] 
        [field: SerializeField] public float SmoothingFactor { get; set; } = 5.0f;
        
        [field: Space(10)]
        [Header("Angle Settings")]
        [Tooltip("The pitch angle where effects start to apply.")]
        [Range(0, 90)] public float startAngle = 60f;
    
        [Tooltip("The pitch angle where effects reach maximum intensity.")]
        [Range(0, 90)] public float endAngle = 70f;
        
        [field: Space(10)]
        [Header("Offset Settings")]
        [Tooltip("Max vertical displacement applied at the endAngle.")]
        [field: SerializeField] public float MaxUpOffset { get; set; } = 0.1f;
    
        [Tooltip("Max backward displacement applied at the endAngle.")]  
        [field: SerializeField] public float MaxBackOffset { get; set; } = -0.1f;
    }
}
