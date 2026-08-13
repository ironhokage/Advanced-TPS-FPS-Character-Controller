using UnityEngine;

namespace Player.PlayerSettings.Movement
{
    [CreateAssetMenu(fileName = "RunSettings", menuName = "Scriptable Objects/RunSettings")]
    public class RunSettings : ScriptableObject
    {
        [Header("Run State Settings")]
        [Tooltip("The base movement speed while running.")]
        [field: SerializeField] public float RunSpeed { get; private set; } = 12.0f;

        [Tooltip("How fast the kccMovement reaches run speed.")]
        [field: SerializeField] public float RunAccel { get; private set; } = 10.0f;

        [Tooltip("How fast the kccMovement stops from a run.")]
        [field: SerializeField] public float RunDeccel { get; private set; } = 9.0f;

        [Tooltip("Whether running should continue automatically without re-pressing the button.")]
        [SerializeField] private bool continuousRun = false;

        // Public getters to allow your state machine or kccMovement script to read these values
        public bool ContinuousRun => continuousRun;
    }
}

