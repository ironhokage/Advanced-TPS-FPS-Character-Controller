using UnityEngine;

[CreateAssetMenu(fileName = "ApexSettings", menuName = "Scriptable Objects/ApexSettings")]
public class ApexSettings : ScriptableObject
{
    [Header("Apex Gravity Settings")]
    [Tooltip("Gravity multiplier applied specifically at the peak of a jump.")]
    [Range(0.01f, 0.25f)]
    [SerializeField] private float apexMultiplier = 0.1f;

    [Tooltip("Speed curve applied during the apex zone.")]
    [SerializeField] private AnimationCurve apexGravitySpeedCurve;

    // Public Getters
    public float ApexMultiplier => apexMultiplier;
    public AnimationCurve ApexGravitySpeedCurve => apexGravitySpeedCurve;
}
