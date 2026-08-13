using UnityEngine;

namespace Player.PlayerSettings.Managers
{
    [CreateAssetMenu(fileName = "HeadBobManagerSettings", menuName = "Scriptable Objects/HeadBobManagerSettings")]
    public class HeadBobManagerSettings : ScriptableObject
    {
        [field: Space(10)]
        [Header("User Preferences")]
        [field: SerializeField] public bool EnableEffect {get; set;} = true;
        
        [field: Space(10)]
        [field: Header("Limit Settings")]
        [field: SerializeField] public float MaxLimit {get; set;} = 0.75f;
        [field: SerializeField] public float MinLimit {get; set;} = 0.25f;
        
        [field: Space(10)]
        [field: Header("Strength Settings")]
        [field: SerializeField] public float HoriStrength {get; set;} = 15f;
        [field: SerializeField] public float VertStrength {get; set;} = 15f;
        
        [field: Space(10)]
        [field: Header("Stiffness Settings")]
        [field: SerializeField] public float HoriStiffness {get; set;} = 15f;
        [field: SerializeField] public float VertStiffness {get; set;} = 15f;
        
        [field: Space(10)]
        [field: Header("Damping Settings")]
        [field: SerializeField] public float HoriDamping {get; set;} = 15f;
        [field: SerializeField] public float VertDamping {get; set;} = 15f;
    }
}
