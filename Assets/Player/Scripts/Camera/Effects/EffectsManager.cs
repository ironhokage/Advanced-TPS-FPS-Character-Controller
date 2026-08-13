using System;
using Cysharp.Threading.Tasks;
using Player.Interfaces.Camera.CameraEffects;
using Player.Interfaces.Movement.Context;
using Player.PlayerSettings.Camera;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Camera.Effects
{
    public class EffectsManager : ICameraEffectsManager
    {
        #region Variables
        public CinemachineCamera VCam { get; set; }
        public Transform CameraTransform { get; set; }
        public Vector2 ZoomInput { get; set; }
        public float CameraPitch { get; set; }
        public Vector3 ActiveOffset { get; set; }
        public Vector3 AnchorPosition { get; set; }
        public float Sensitivity { get; set; }

        #endregion
        
        #region Sub-Module Interfaces
        private readonly IDynamicFOV _dynamicFOV = new DynamicCameraFOV();
        private readonly ICameraZoom _cameraZoom = new CameraZoom();
        private readonly IBodyImmersiveFOV _bodyImmersiveFOV = new ImmersiveFOV();

        #endregion
        
        private static float ClampedDeltaTime => PlayerContext.State != null ? PlayerContext.State.ClampedDeltaTime : Time.deltaTime;
        
        #region Instantiate Zoom Settings
        public async UniTask<CameraZoomSettings> InstantiateAsyncZoomSettings()
        {
            try
            {
                var fovHandle = Addressables.LoadAssetAsync<CameraZoomSettings>("CameraZoomSettings");
                _cameraZoom.ZoomSettings = await fovHandle.ToUniTask();
                
                return _cameraZoom.ZoomSettings;
            }
            catch (Exception e)
            {
                Debug.LogError("Camera zoom settings could not be instantiated: " + e.Message);
                throw;
            }
        }

        #endregion
        
        #region Instantiate Dynamic Camera FOV
        public async UniTask<CameraFOVSettings> InstantiateAsyncFOVSettings()
        {
            try
            {
                var fovHandle = Addressables.LoadAssetAsync<CameraFOVSettings>("CameraFOVSettings");
                _dynamicFOV.CameraFOVSettings = await fovHandle.ToUniTask();
                
                return _dynamicFOV.CameraFOVSettings;
            }
            catch (Exception e)
            {
                Debug.LogError("Camera fov settings could not be instantiated: " + e.Message);
                throw;
            }
        }
        #endregion

        #region Instantiate Immersive FOV
        public async UniTask<ImmersiveFOVSettings> InstantiateAsyncImmersiveFOVSettings()
        {
            try
            {
                var immersiveHandle = Addressables.LoadAssetAsync<ImmersiveFOVSettings>("ImmersiveFOVSettings");
                
                _bodyImmersiveFOV.ImmersiveFOVSettings = await immersiveHandle.ToUniTask();
                return _bodyImmersiveFOV.ImmersiveFOVSettings;
            }
            catch (Exception e)
            {
                Debug.LogError("Immersive FOV settings could not be instantiated: " + e.Message);
                throw;
            }
        }
        #endregion
        
        #region InitializeMovementDependencies Camera Effects
        public void InitializeEffects()
        {
            _cameraZoom.InitializeFOV(VCam);
            _dynamicFOV.InitializeDynamicFOV(VCam);
            _bodyImmersiveFOV.Initialize(CameraTransform, VCam);
        }
        #endregion
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateEffects()
        {
            _cameraZoom.HandleScrollInput(ZoomInput);
            _cameraZoom.UpdateFOV();
            Sensitivity = _cameraZoom.ReduceSensitivity();
            
            _dynamicFOV.CacheReferences(ClampedDeltaTime);
        }
        
        public void LateUpdateEffects()
        {
            ActiveOffset = _bodyImmersiveFOV.DepthOffsetFOVModifier(CameraPitch, AnchorPosition, ClampedDeltaTime);
            _dynamicFOV.UpdateCameraFOV(_bodyImmersiveFOV.PitchFactor);
        }
        
    }
}
