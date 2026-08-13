using UnityEngine;

namespace Player.Camera.SOs
{
    [CreateAssetMenu(fileName = "PositionSettings", menuName = "Scriptable Objects/PositionSettings")]
    public class CameraPositionSettings : ScriptableObject
    {
        [field: Header("FPS Settings")]
        [field: SerializeField] public float FPSEyeOffset { get; set; } = 1.55f;
        
        [field: Space(10)]
        [field: Header("TPS Settings")]
        [field: SerializeField] public float TpsCameraPositionSmoothing { get; set; } = 5.0f;
        [field: SerializeField] public float TPSEyeOffset { get; set; } = 1.55f;
        [field: SerializeField] public float TPSXAxisOffset { get; set; } = 1.55f;
        [field: SerializeField] public float TPSZAxisOffset { get; set; } = -3.55f;
        
        [field: Space(10)]
        [field: Header("General Settings")]
        [field: SerializeField] public float ForwardDistance { get; set; } = 5f;
    }
}
