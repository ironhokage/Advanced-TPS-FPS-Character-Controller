using Player.PlayerSettings.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Interfaces.Camera.CameraEffects
{
    public interface IDynamicFOV
    {
        #region FOV Multipliers
        float FOVSpeedMultiplier { get; set; }
        float FOVSlopeScalingMultiplier { get; set; }
        float FOVStateMultiplier { get; set; }
        #endregion
        
        #region Functions
        void InitializeDynamicFOV(CinemachineCamera vCam);
        void UpdateCameraFOV(float value);
        void CacheReferences(float delta);
        #endregion
        
        CameraFOVSettings CameraFOVSettings { get; set; }
    }
}
