using UnityEngine;
using UnityEngine.Serialization;

namespace Player.PlayerSettings.Physics
{
    [CreateAssetMenu(fileName = "DragSettings", menuName = "Scriptable Objects/DragSettings")]
    public class DragSettings : ScriptableObject
    {
        #region Base Drag Values
        
        [field: Header("Base Settings")]
        [field: SerializeField] public float GroundDrag { get; private set; } = 8.25f;
        [field: SerializeField] public float AirDrag { get; private set; } = 0.2f;
        
        [field: Header("Clamp Limits")]
        [field: SerializeField] public float MinDrag { get; private set; } = 2f;
        [field: SerializeField] public float MaxDrag { get; private set; } = 15f;
        [field: SerializeField] public float FinalMultCap { get; private set; } = 1.5f;
        
        #endregion
        
        [Header("Drag Blend Curve")]
        public AnimationCurve directionalDragCurve = AnimationCurve.EaseInOut(0f, 1.25f, 1f, 1f);
    }
}