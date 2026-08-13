using UnityEngine;

namespace Player.PlayerSettings.Jump
{
    [CreateAssetMenu(fileName = "JumpSettings", menuName = "Scriptable Objects/JumpSettings")]
    public class JumpSettings : ScriptableObject
    {
        [Header("Physics & Timing")]
        [Tooltip("The peak height of the jump in meters.")]
        [field: SerializeField] public float JumpHeight { get; private set; } = 20.0f;

        [Tooltip("Time taken to reach the apex of the jump.")]
        [field: SerializeField] public float JumpTimeToPeak { get; private set; } = 0.4f;

        [Tooltip("Time taken to fall back to ground level from the apex.")]
        [field: SerializeField] public float JumpTimeToFall { get; private set; } = 0.3f;

        [Tooltip("Cooldown time between jumps.")]
        [field: SerializeField] public float JumpCooldown { get; private set; } = 0.085f;

        [Tooltip("How early a kccMovement can press jump before hitting the ground and still have it trigger.")]
        [field: SerializeField] public float JumpBufferTime { get; private set; } = 0.2f;

        [Header("Air Control")]
        [Tooltip("Reduces/increases horizontal movement control while airborne.")]
        [Range(0.1f, 1.0f)]
        [field: SerializeField] public float InAirInputMultiplier { get; private set; } = 1.0f;

        [Tooltip("Sensitivity/Boost applied at the apex of the jump.")]
        [Range(0.01f, 0.25f)]
        [field: SerializeField] public float ApexMultiplier { get; private set; } = 0.1f;

        [Header("Special States")]
        [Tooltip("Initial velocity applied when the jump is triggered.")]
        [field: SerializeField] public float JumpUpSpeed { get; private set; } = 10f;

        [Tooltip("Can the kccMovement jump while in a sliding state?")]
        [field: SerializeField] public bool AllowJumpingWhenSliding { get; private set; }
        
    }
}
