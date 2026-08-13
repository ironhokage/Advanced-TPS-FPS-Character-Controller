using UnityEngine;

namespace Player.PlayerSettings.Camera
{
    [CreateAssetMenu(fileName = "CameraFOVSettings", menuName = "Scriptable Objects/CameraFOVSettings")]
    public class CameraFOVSettings : ScriptableObject
    {
        [field: Header("Feature Status")]
        [Tooltip("If we don't want to use the feature, we turn it off.")] 
        public bool enabled = true;
        
        [field: Space(10)]
        [Header("Interpolation Settings")]
        [Tooltip("How quickly the camera position follows the target offset.")] 
        [field: SerializeField] public float SmoothingFactor { get; set; } = 5.0f;
        [Tooltip("Smoothing time for FOV transitions.")]
        [field: SerializeField] public float FOVSmoothTime { get; set; } = 0.2f;
        [Tooltip("Smoothing time for FOV curves.")]
        [field: SerializeField] public float FOVSmoothSlopeCurveTime { get; set; } = 0.2f;
        
        [field: Space(10)]
        [Header("Zoom Settings")] 
        [Tooltip("Additional Field of View added when looking down/up beyond threshold.")]
        [field: SerializeField] public float MaxPitchFOVOffset { get; set; } = 20.0f;
        
        [field: Space(10)]
        [field: Header("FOV Slope Curve Settings")]
        [field: SerializeField] public AnimationCurve fovDownhillSlopeStateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [field: SerializeField] public AnimationCurve fovUphillSlopeStateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [field: Space(10)]
        [field: Header("FOV Move State Curve Settings")]
        [field: SerializeField] public AnimationCurve fovRunStateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [field: SerializeField] public AnimationCurve fovCrouchStateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [field: Space(10)]
        [field: Header("FOV Move Direction Curve Settings")]
        [field: SerializeField] public AnimationCurve fovForwardStopCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [field: SerializeField] public AnimationCurve fovBackwardStopCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }
}