using Player.Interfaces.Camera.CameraEffects;
using Player.PlayerSettings.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Camera.Effects
{
    public class CameraZoom : ICameraZoom
    {
        private enum ZoomLevel
        {
            None = 0, 
            Small = 1, 
            Medium = 2,
            Large = 3
        }
        
        private float _zoomLockTimer;
        private float _currentVelocity;
        private CinemachineCamera _vCam;
        public bool IsZooming => _zoomLockTimer > 0;
        public CameraZoomSettings ZoomSettings { get; set; }
        private ZoomLevel _currentZoomLevel = ZoomLevel.None;

        public void InitializeFOV(CinemachineCamera vCam)
        {
            _vCam = vCam;
            
            if(vCam != null || ZoomSettings != null) 
                vCam.Lens.FieldOfView = ZoomSettings.defaultFOV;
        }
        
        public void HandleScrollInput(Vector2 zoomInput)
        {
            if(_vCam == null || ZoomSettings == null) return;
            if (_zoomLockTimer > 0) _zoomLockTimer -= Time.deltaTime;

            var input = zoomInput.y;

            if (!IsZooming && Mathf.Abs(input) > 0.01f)
            {
                var direction = input > 0 ? 1 : -1;
                
                var nextState = (int)_currentZoomLevel + direction;
                _currentZoomLevel = (ZoomLevel)Mathf.Clamp(nextState, 0, 3);
                _zoomLockTimer = ZoomSettings.ZoomDuration;
            }
        }

        public void UpdateFOV()
        {
            if (_vCam == null || ZoomSettings == null) return;
            var targetFOV = SelectTargetFOV();
        
            _vCam.Lens.FieldOfView = Mathf.SmoothDamp(
                _vCam.Lens.FieldOfView, 
                targetFOV, 
                ref _currentVelocity, 
                ZoomSettings.ZoomDuration
            );
        }
        
        public float ReduceSensitivity()
        {
            if (_vCam == null || ZoomSettings == null) return 1f;
            
            var t = Mathf.InverseLerp(ZoomSettings.defaultFOV, ZoomSettings.defaultFOV * ZoomSettings.bigZoomMult, _vCam.Lens.FieldOfView);
            return Mathf.Lerp(1.0f, 0.25f, t);
        }

        #region Private Methods
        private float SelectTargetFOV()
        {
            return _currentZoomLevel switch
            {
                ZoomLevel.Small => ZoomSettings.defaultFOV * ZoomSettings.smallZoomMult,
                ZoomLevel.Medium => ZoomSettings.defaultFOV * ZoomSettings.mediumZoomMult,
                ZoomLevel.Large => ZoomSettings.defaultFOV * ZoomSettings.bigZoomMult,
                _ => ZoomSettings.defaultFOV
            };
        }
        #endregion
        
    }
}
