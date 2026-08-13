using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Player.PlayerSettings.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Interfaces.Camera.CameraEffects
{
    public interface ICameraEffectsManager
    {
        #region Instantiating
        UniTask<CameraFOVSettings> InstantiateAsyncFOVSettings();
        UniTask<CameraZoomSettings> InstantiateAsyncZoomSettings();
        UniTask<ImmersiveFOVSettings> InstantiateAsyncImmersiveFOVSettings();
        #endregion

        #region General Functions
        void UpdateEffects();
        void LateUpdateEffects();
        void InitializeEffects();
        #endregion

        #region Important Variables
        CinemachineCamera VCam { get; set; }
        Transform CameraTransform { get; set; }
        Vector2 ZoomInput { get; set; }
        float CameraPitch { get; set; }
        Vector3 ActiveOffset { get; set; }
        Vector3 AnchorPosition { get; set; }
        float Sensitivity { get; set; }
        #endregion



    }
}
