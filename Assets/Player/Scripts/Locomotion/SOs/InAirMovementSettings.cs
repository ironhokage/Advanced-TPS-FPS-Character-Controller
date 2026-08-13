using UnityEngine;

namespace Player.Scripts.PlayerSettings.Movement
{
    [CreateAssetMenu(fileName = "InAirMovementSettings", menuName = "Scriptable Objects/InAirMovementSettings")]
    public class InAirMovementSettings : ScriptableObject
    {
        [Header("In Air Walk Settings")]
        [field: SerializeField, Tooltip("The base movement speed while walking in the air.")] 
        public float AirWalkSpeed { get; private set; } = 9.0f;

        [field: SerializeField, Tooltip("How fast the kccMovement reaches walk speed while in the air.")] 
        public float AirWalkAccel { get; private set; } = 11.0f;

        [field: SerializeField, Tooltip("How fast the kccMovement stops from a walk while in the air.")] 
        public float AirWalkDecel { get; private set; } = 10.0f;
        
        [Header("Air Movement Configuration")]
        [field: SerializeField, Tooltip("The absolute maximum speed the kccMovement can reach via air steering.")] 
        public float MaxAirMoveSpeed { get; private set; } = 15f;

        [field: SerializeField, Tooltip("General acceleration multiplier applied to air movement.")] 
        public float AirAccelerationSpeed { get; private set; } = 15f;

        [field: SerializeField, Tooltip("General deceleration rate when no input is provided in the air.")] 
        public float AirDecelerationSpeed { get; private set; } = 15f;
        
        [field: SerializeField, Tooltip("Curve defining movement speed while airborne.")]
        public AnimationCurve InAirMoveSpeedCurve { get; private set; }
        
        [field: SerializeField, Tooltip("Curve defining vertical speed during a fall.")]
        public AnimationCurve FallSpeedCurve { get; private set; }

        [field: SerializeField, Tooltip("Time in seconds before the kccMovement can register another ground hit.")]
        public float HitGroundCooldown { get; private set; } = 0.1f;
    }
}