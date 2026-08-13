using UnityEngine;

namespace Player.PlayerSettings.Jump
{
    [CreateAssetMenu(fileName = "CoyoteSettings", menuName = "Scriptable Objects/CoyoteSettings")]
    public class CoyoteSettings : ScriptableObject
    {
        [Header("Coyote Time Settings")]
        [Tooltip("The grace period (in seconds) during which the kccMovement can still jump after leaving a ledge.")]
        [SerializeField] private float coyoteTime = 0.25f;

        [Tooltip("The horizontal speed threshold required to trigger coyote time logic.")]
        [SerializeField] private float coyoteSpeedThreshold = 20.45f;

        // Public Getters
        public float CoyoteTime => coyoteTime;
        public float CoyoteSpeedThreshold => coyoteSpeedThreshold;
    }
}
