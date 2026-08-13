using UnityEngine;

namespace Player.PlayerSettings.Managers
{
    [CreateAssetMenu(fileName = "GravitySettings", menuName = "Scriptable Objects/GravitySettings")]
    public class GravitySettings : ScriptableObject
    {
        // --- CONSTANTS ---
        public const float HANG_TIME = 0.15f;
        public const float VELOCITY_BAND = 0.1f;
        public const float INVARIABLE_DELTA = 0.0167f;
        public const float MIN_THRESHOLD = -0.55f;
        public const float MAX_THRESHOLD = 0.25f;
        public const float BASE_OFFSET = -0.05f;
        public const float MAX_JUMP_RELEASE_TIME = 0.05f;

        // --- ENUMS ---
        public enum ApexPhase { None, Entering, Leaving }
        // --- EXPORTS (Serialized Fields with Tooltips) ---
        [Header("References")]
        [Tooltip("Reference to the main character controller component.")]
        public CharacterController playChar; // Equivalent to CharacterBody3D

        // --- EXPORTS (Serialized Fields with Tooltips) ---
        [Header("Debug Settings")]
        [Tooltip("Enable general debug logging.")]
        public bool debug = true;

        [Tooltip("Enable specific debug logging for jump apex phases.")]
        public bool debugApex = true;
    }
}
