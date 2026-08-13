using UnityEngine;

namespace Player.PlayerSettings.Movement
{
    [CreateAssetMenu(fileName = "WalkSettings", menuName = "Scriptable Objects/WalkSettings")]
    public class WalkSettings : ScriptableObject
    {
        [Header("Walk Settings")]
        [Tooltip("The base movement speed while walking.")]
        [field: SerializeField] public float WalkSpeed { get; private set; } = 9.0f;

        [Tooltip("How fast the kccMovement reaches walk speed.")]
        [field: SerializeField] public float WalkAccel { get; private set; } = 11.0f;

        [Tooltip("How fast the kccMovement stops from a walk.")]
        [field: SerializeField] public float WalkDecel { get; private set; } = 10.0f;
        
    }
}
