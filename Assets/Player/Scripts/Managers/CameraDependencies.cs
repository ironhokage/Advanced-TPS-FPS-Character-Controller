using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.Camera.Core;
using Player.Camera.Effects;
using Player.Input;
using Player.Interfaces.Movement.Context;
using Player.Managers.Interfaces;
using Player.Scripts.Camera.Core;
using Player.Scripts.Camera.Effects;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Managers
{
    public class CameraDependencies : ICameraDependency
    {
        private readonly CameraInput _cameraInput = new();
        private readonly CameraRotation _cameraRotation = new();
        private readonly CameraPositionManager _cameraPosition = new();
        private readonly EffectsManager _cameraEffectsManager = new();
        private readonly List<CinemachineCamera> _cameras = new();  
        
        private CinemachineCamera _activeCamera;
        
        private CinemachineCamera _fpsCamera;
        public CinemachineCamera _tpsCamera;

        public bool IsCameraActive { get; set; }

        public float TargetYaw { get; set; }

        public KinematicCharacterMotor CharacterMotor { get; set; }
        
        private Vector2 _lookVector;
        private ICameraDependency _iCameraDependencyImplementation;
        private static Vector3 CharacterUp => PlayerContext.Orientation.CharacterUp;
        
        public async UniTask InitializeCameraDependencies(CancellationTokenSource ctx)
        {
            try
            {
                // When early canceling the load the ctx will be flagged as canceled thus making the code stop waiting for this to load
                await _cameraInput.InstantiateAsync().AttachExternalCancellation(ctx.Token);
                await _cameraRotation.InstantiateAsync().AttachExternalCancellation(ctx.Token);
                await _cameraPosition.InstantiateAsync().AttachExternalCancellation(ctx.Token);

                await _cameraEffectsManager.InstantiateAsyncZoomSettings().AttachExternalCancellation(ctx.Token);
                await _cameraEffectsManager.InstantiateAsyncFOVSettings().AttachExternalCancellation(ctx.Token);
                await _cameraEffectsManager.InstantiateAsyncImmersiveFOVSettings().AttachExternalCancellation(ctx.Token);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            
            SetCursorState(true);
            _cameraPosition.CharUp = CharacterUp;
            _cameraEffectsManager.InitializeEffects();
            IsCameraActive = true;
        }

        public void UpdateDependency(UniversalInputManager inputManager, bool isInFPS)
        {
            if(!IsCameraActive) return;
            
            _lookVector = _cameraInput.InputHandler();
            
            _cameraEffectsManager.ZoomInput = inputManager.ZoomInput;
            _cameraEffectsManager.CameraPitch = _cameraRotation.SmoothedPitch;
            
            _cameraEffectsManager.UpdateEffects();
            _cameraRotation.HandleRotationInput(_lookVector * _cameraEffectsManager.Sensitivity);
            TargetYaw = _cameraRotation.CameraUpdateRotation(_activeCamera.gameObject, isInFPS);
        }
        
        public void LateUpdateDependency(bool isInFPS)
        {
            if(!IsCameraActive) return;
            _cameraPosition.ComputeCameraPositionCoordinates(isInFPS, _activeCamera.gameObject, CharacterMotor.transform, _cameraRotation.SmoothedYaw, _cameraRotation.SmoothedPitch);
            _cameraEffectsManager.LateUpdateEffects();
        }
        
        public void GetCameraReferences(List<CinemachineCamera> cameras)
        {
            foreach (var cam in cameras)
            {
                if (cam.CompareTag("FPSVCam"))
                {
                    _fpsCamera = cam;
                }
                else if (cam.CompareTag("TPSVCam"))
                {
                    _tpsCamera = cam;
                }
            }
        }
        
        private void SwitchCam(CinemachineCamera cinemachineCamera) 
        {
            cinemachineCamera.Priority = 99;
            _activeCamera = cinemachineCamera;
            foreach (var c in _cameras.Where(c => c != cinemachineCamera && c.Priority != 0)) { c.Priority = 0; }
        }

        private bool IsActiveCamera(CinemachineCamera cinemachineCamera) => cinemachineCamera == _activeCamera;
        
        public void OnCamSwitchTriggered(ref bool isInFPS)
        {
            if (IsActiveCamera(_fpsCamera))
            {
                SwitchCam(_tpsCamera);
                isInFPS = false;
                SnapTpsCameraBehindCharacter(); 
            }
            else if (IsActiveCamera(_tpsCamera))
            {
                SwitchCam(_fpsCamera);
                isInFPS = true;
            }
            CameraSwitcher(isInFPS);
        }
        
        
        public void CameraSwitcher(bool isInFPS)
        {
            _cameras.Clear();
            
            _activeCamera = isInFPS ? _fpsCamera : _tpsCamera;
            _cameraEffectsManager.VCam = _activeCamera;
            
            _cameras.Add(_fpsCamera);
            _cameras.Add(_tpsCamera);
            
            SwitchCam(_activeCamera);
        }
        
        private void SnapTpsCameraBehindCharacter()
        {
            if (_tpsCamera == null || CharacterMotor == null)
                return;

            var camRotSett = _cameraRotation.RotationSettings;

            var charForward = CharacterMotor.CharacterForward;
            var charUp = CharacterMotor.CharacterUp;
            
            var flatForward = Vector3.ProjectOnPlane(charForward, charUp).normalized;
            if (flatForward.sqrMagnitude < 0.001f)
                flatForward = Vector3.ProjectOnPlane(charUp, charUp).normalized;
            var targetYaw = Mathf.Atan2(flatForward.x, flatForward.z) * Mathf.Rad2Deg;
            
            _cameraRotation.ResetTpsOrientation(targetYaw, camRotSett.TargetPitch);
            
            _cameraPosition.ComputeCameraPositionCoordinates(
                false,                        
                _tpsCamera.gameObject,
                CharacterMotor.transform,
                0f,                 
                camRotSett.TargetPitch);
        }
        
        private static void SetCursorState(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = false;
        }
    }
}
