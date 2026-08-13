using Player.PlayerSettings.Managers;
using UnityEngine;

namespace Player.Scripts.Managers.Physics
{
    public class PlayerGravityManager 
    {
        [Tooltip("Reference to the GravitySettings ScriptableObject.")]
        public GravitySettings Settings;

        // --- RUNTIME VARIABLES (State: Apex & Buffer) ---
        [Header("Runtime State")]
        private GravitySettings.ApexPhase apexPhase = GravitySettings.ApexPhase.None;
        private float currentApexCandidate = 0.0f;
        private bool inApexZone = false;
        private bool canEnterApexZone = true;
        private float hangTimer = 0.0f;
        private float fallThreshold = 0.0f;

        // --- RUNTIME VARIABLES (Movement Calculations) ---
        private float horizontalSpeed = 0.0f;
        private float horizontalFactor = 0.0f;
        private float maxHorizontalSpeed = 0.0f;
        private float effectiveHangTime = 0.0f;
        private float effectiveJumpGravity = 0.0f;
        private float effectiveApexGravity = 0.0f;

        // --- RUNTIME VARIABLES (Physics & Gravity) ---
        private float gravityStep = 0.0f;
        private float blendFactor = 0.0f;
        private float apexNormal = 0.0f;
        private float fallNormal = 0.0f;
        private float apexCurve = 0.0f;
        private float fallCurve = 0.0f;
        private float apexFactor = 0.0f;
        private float holdPercentage = 0.0f;
        private float gravityMultiplier = 0.0f;

   
    }
}
