using UnityEngine;

namespace Player.PlayerSettings.Movement
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Scriptable Objects/MovementSettings")]
    public class MovementSettings : ScriptableObject
    {
        // =========================
        // SPEED
        // =========================
        [field: Space(10)]
        [field: Header("Speed")] 
        [field: SerializeField] public float MinMovementSpeed { get; private set; } = 2f;
        [field: SerializeField] public float MaxMovementSpeed { get; private set; } = 30f;
        [field: SerializeField] public float CurrentMovementSpeed { get; set; } = 20f;
        [field: SerializeField] public float DragValue { get; set; } = 2.5f;
        [field: SerializeField] public float SoftClampPower { get; set; } = 2.55f;

        // =========================
        // ACCELERATION
        // =========================
        [field: Space(10)]
        [field: Header("Acceleration")] 
        [field: SerializeField] public float CurrentAccelerationSpeed { get; set; } = 18f;
        [field: SerializeField] public float CurrentDecelerationSpeed { get; set; } = 14f;
        [field: SerializeField] public AnimationCurve accelerationResponseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [field: SerializeField] public AnimationCurve decelerationResponseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        // =========================
        // CORE RESPONSIVENESS
        // =========================
        [field: Space(10)]
        [field: Header("Responsiveness")] 
        [field: SerializeField] public float BaseResponsiveness { get; private set; } = 8f;
        [field: SerializeField] public float MinResponsiveness { get; private set; } = 0.1f;
        [field: SerializeField] public float HighSpeedDampening { get; private set; } = 0.6f;
        [field: SerializeField] public float DirectionVsPivotWeight { get; set; } = 0.4f;
        [field: SerializeField] public float AccelerationSmoothingFactor { get; set; } = 0.8f;

        // =========================
        // DIRECTIONAL CONTROL
        // =========================
        [field: Space(10)]
        [field: Header("Directional Control")]
        [field: SerializeField] public float SmoothedMovement { get; set; } = 5f;
        [field: SerializeField] public AnimationCurve directionalForwardsResponsivenessCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1);
        [field: SerializeField] public AnimationCurve directionalBackwardsResponsivenessCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1);
        [field: SerializeField] public AnimationCurve directionalForwardsSpeedCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1);
        [field: SerializeField] public AnimationCurve directionalBackwardsSpeedCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1);
        [field: SerializeField] public AnimationCurve directionChangeSpeedCurve = AnimationCurve.EaseInOut(0, 0.2f, 1, 1);

        // =========================
        // PIVOTING
        // =========================
        [field: Space(10)]
        [field: Header("Pivoting")] 
        [field: SerializeField] public float DirectionChangeEnterThreshold { get; private set; } = 0.5f;
        [field: SerializeField] public AnimationCurve pivotCurve = AnimationCurve.EaseInOut(0, 0.1f, 1, 1);
        [field: SerializeField] public AnimationCurve speedWeightCurve = AnimationCurve.EaseInOut(0, 0.1f, 1, 1);

        // =========================
        // SLOPE
        // =========================
        [field: Space(10)]
        [field: Header("Slope")] 
        [field: SerializeField] public float MaxSlopeAngle { get; set; } = 45f;
        [field: SerializeField] public float TransitionValue { get; set; } = 0.5f;
        [field: SerializeField] public float UphillBackwardResponsiveness { get; set; } = 0.7f;
        [field: SerializeField] public float UphillForwardResponsiveness { get; set; } = 0.7f;
        [field: SerializeField] public float DownhillBackwardResponsiveness { get; set; } = 0.9f;
        [field: SerializeField] public float DownhillForwardResponsiveness { get; set; } = 0.9f;
        [field: SerializeField] public float UphillBackwardSpeedFactor { get; set; } = 0.8f;
        [field: SerializeField] public float DownhillBackwardSpeedFactor { get; set; } = 0.8f;
        [field: SerializeField] public float UphillForwardSpeedFactor { get; set; } = 0.8f;
        [field: SerializeField] public float DownhillForwardSpeedFactor { get; set; } = 0.8f;
        
        // =========================
        // START AND TURN BOOST
        // =========================
        [field: Space(10)]
        [field: Header("Boost Curves")]
        [field: SerializeField] public AnimationCurve startBoostCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [field: SerializeField] public AnimationCurve turnBoostCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [field: SerializeField] public AnimationCurve inputResponseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [field: SerializeField] public AnimationCurve directionalBlendCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
    }
}