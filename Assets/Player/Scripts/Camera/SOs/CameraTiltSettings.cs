using UnityEngine;

[CreateAssetMenu(fileName = "CameraTiltSettings", menuName = "Scriptable Objects/CameraTiltSettings")]
public class CameraTiltSettings : ScriptableObject
{
    [Header("Master Toggles")]
    [Tooltip("Enable procedural tilting forward/backward based on movement velocity.")]
    [SerializeField] private bool enableForwardTilt = true;
    [Tooltip("Enable procedural tilting side-to-side based on strafing velocity.")]
    [SerializeField] private bool enableSideTilt = true;

    [Header("Forward Tilt (Pitch)")]
    [Tooltip("The higher this value, the less the camera tilts for a given speed (Velocity / Divider).")]
    [Range(0.1f, 400.0f)][SerializeField] private float forwardMoveTiltDivider = 260.0f;
    [Tooltip("The absolute maximum degrees the camera can tilt forward or backward.")]
    [Range(0.0f, 10.0f)][SerializeField] private float forwardMoveMaxTiltVal = 2.0f;
    [Tooltip("How quickly the tilt reaches its target rotation.")]
    [Range(0.0f, 10.0f)][SerializeField] private float forwardMoveTiltSpeed = 5.0f;

    [Header("Side Tilt (Roll)")]
    [Tooltip("Divider for side movement. Lower values result in more aggressive rolling.")]
    [Range(0.1f, 10.0f)][SerializeField] private float sideMoveTiltDivider = 1.75f;
    [Tooltip("Speed of the roll interpolation.")]
    [Range(0.0f, 24.0f)][SerializeField] private float sideMoveTiltSpeed = 10.0f;
    [Tooltip("The maximum roll angle in degrees when strafing.")]
    [Range(0.0f, 12.0f)][SerializeField] private float sideMoveMaxTiltVal = 0.85f;

    [Header("Y-Axis Nudge")]
    [Tooltip("Small angle adjustment applied to the Y-axis when moving forward.")]
    [Range(1.0f, 5.65f)][SerializeField] private float smallForwardAngle = 2.5f;
    [Tooltip("Small angle adjustment applied to the Y-axis when moving backward.")]
    [Range(1.0f, 5.65f)][SerializeField] private float smallBackwardAngle = 2.5f;

    // Getters
    public bool EnableForwardTilt => enableForwardTilt;
    public bool EnableSideTilt => enableSideTilt;
    public float ForwardMoveTiltDivider => forwardMoveTiltDivider;
    public float ForwardMoveMaxTiltVal => forwardMoveMaxTiltVal;
    public float ForwardMoveTiltSpeed => forwardMoveTiltSpeed;
    public float SideMoveTiltDivider => sideMoveTiltDivider;
    public float SideMoveTiltSpeed => sideMoveTiltSpeed;
    public float SideMoveMaxTiltVal => sideMoveMaxTiltVal;
    public float SmallForwardAngle => smallForwardAngle;
    public float SmallBackwardAngle => smallBackwardAngle;
}
