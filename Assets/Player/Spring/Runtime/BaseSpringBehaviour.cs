using UnityEngine;

namespace Player.Spring.Runtime
{
    public class BaseSpringBehaviour : MonoBehaviour
    {
        [Range(0.01f, 100f)]
        [field: SerializeField] protected float Damping { get; set; } = 26;
        
        [Range(0, 500)]
        [field: SerializeField] protected float Stiffness { get; set; } = 169f;
    }
}
