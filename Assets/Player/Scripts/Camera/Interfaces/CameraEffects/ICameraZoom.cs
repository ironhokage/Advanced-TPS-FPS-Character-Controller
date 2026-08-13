using Player.PlayerSettings.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Interfaces.Camera.CameraEffects
{
    public interface ICameraZoom
    {
        CameraZoomSettings ZoomSettings { get; set; }
        
        #region Functions
        void InitializeFOV(CinemachineCamera vCam);
       float ReduceSensitivity();
        void UpdateFOV();
        void HandleScrollInput(Vector2 zoomInput);
        #endregion
    }
}
