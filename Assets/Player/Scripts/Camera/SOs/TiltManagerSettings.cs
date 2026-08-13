using UnityEngine;
using UnityEngine.Serialization;

namespace Player.PlayerSettings.Managers
{
    [CreateAssetMenu(fileName = "TiltManagerSettings", menuName = "Scriptable Objects/TiltManagerSettings")]
    public class TiltManagerSettings : ScriptableObject
    {
        [field: Space(10)]
        [Header("User Preferences")]
        [field: SerializeField] public bool EnableEffect {get; set;} = true;
        
        [field: Space(10)]
        [field: Header("Limit Settings")]
        [field: SerializeField] public float MaxLimit {get; set;} = 0.75f;
        
        [field: Space(10)]
        [field: Header("Strength Settings")]
        [field: SerializeField] public float PerpendicularStrength {get; set;} = 15f;
        
        [field: Space(10)]
        [field: Header("Stiffness Settings")]
        [field: SerializeField] public float TiltStiffness {get; set;} = 15f;
        
        [field: Space(10)]
        [field: Header("Damping Settings")]
        [field: SerializeField] public float TiltDamping {get; set;} = 15f;
        
    }
}
