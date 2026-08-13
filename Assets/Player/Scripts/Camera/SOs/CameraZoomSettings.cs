using UnityEngine;

namespace Player.PlayerSettings.Camera
{
    [CreateAssetMenu(fileName = "CameraZoomSettings", menuName = "Scriptable Objects/CameraZoomSettings")]
    public class CameraZoomSettings : ScriptableObject
    {  
        [Header("Field of View")]
        [Tooltip("The default FOV value.")]
        [Range(60f, 110f)] 
        public float defaultFOV = 75f;
    
        [Header("Zoom Multipliers")]
        public float smallZoomMult = 0.75f;
        public float mediumZoomMult = 0.45f;
        public float bigZoomMult = 1.25f;
    
        [Header("Field of View (FOV) Limits")]
        [Tooltip("The lowest possible FOV value allowed.")]
        [Range(0.0f, 180.0f)]
        [SerializeField] private float minFovVal = 10.0f;

        [Tooltip("The highest possible FOV value allowed.")]
        [Range(0.0f, 180.0f)]
        [SerializeField] private float maxFovVal = 170.0f;

        [Header("Zoom Settings")]
        [Tooltip("The FOV value to set when the kccMovement is zoomed in.")]
        [Range(-180.0f, 180.0f)]
        [SerializeField] private float zoomVal = 40.0f;

        [Tooltip("The duration (in seconds) it takes to complete the zoom transition.")]
        [Range(0.1f, 1.0f)]
        [SerializeField]
        private float zoomDuration = 0.35f;

        // Getters for implementation logic
        public float DefaultFOVVal => defaultFOV;
        public float MinFovVal => minFovVal;
        public float MaxFovVal => maxFovVal;
        public float ZoomVal => zoomVal;
        public float ZoomDuration => zoomDuration;

    }
}
