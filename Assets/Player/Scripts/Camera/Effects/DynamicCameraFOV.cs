using Player.Camera.Statics;
using Player.Interfaces.Camera.CameraEffects;
using Player.Interfaces.Movement.Context;
using Player.Managers.Helpers;
using Player.PlayerSettings.Camera;
using Player.Scripts.StateMachines.Base;
using Player.StateMachines.CameraSM_s;
using Player.StateMachines.MovementStates;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Scripts.Camera.Effects
{
    public class DynamicCameraFOV : IDynamicFOV
    {
        #region Character Movement 
        private float _previousCharacterDirection;
        private float _movementCurve;
        private float _currentSpeed;
        private readonly MovementStateSM _moveState = new();
        #endregion

        #region Camera Properties
        private CinemachineCamera _vCam;
        #endregion

        #region Misc
        private float _clampedDeltaTime;
        private Vector3 _activeOffset;
        private PlayerGroundMovementHelper.SlopeData _playerSlopeFactor;
        #endregion

        #region Interface Variables
        private static IState ActiveState => PlayerContext.State?.PlayerState?.CurrentState?.State;
        private static PlayerGroundMovementHelper.SlopeData PlayerSlopeData => GroundMovementContext.DirectionHooks != null ? GroundMovementContext.DirectionHooks.SlopeData : default;
        private static float SpeedRatio => GroundMovementContext.MovementHooks != null
            ? GroundMovementContext.MovementHooks.SpeedRatio
            : 0f;
        private static float ForwardDirection => GroundMovementContext.MovementHooks != null
            ? GroundMovementContext.MovementHooks.LocalForwardVelocity
            : 0f;
        #endregion
        
        #region Interface Implementation

        public float FOVSpeedMultiplier { get; set; } = 1f;
        public float FOVSlopeScalingMultiplier { get; set; } = 1f;
        public float FOVStateMultiplier { get; set; } = 1f;
        public CameraFOVSettings CameraFOVSettings { get; set; }
        #endregion
        
        public void InitializeDynamicFOV(CinemachineCamera vCam)
        {
            _vCam = vCam;
        }
        public void CacheReferences(float delta)
        {
            _currentSpeed = SpeedRatio;
            _playerSlopeFactor = PlayerSlopeData;
            _clampedDeltaTime = delta;
        }
        
        public void UpdateCameraFOV(float value)
        {   
            if(CameraFOVSettings == null || _vCam == null) return;
            
            var targetFOV = _vCam.Lens.FieldOfView + CameraFOVSettings.MaxPitchFOVOffset * value;
            var finalFOV = UpdateFinalFOV(targetFOV);
            
            _vCam.Lens.FieldOfView = CameraFOVSettings ? InterpolationHelperFunctions.ValueInterpolation(_vCam.Lens.FieldOfView,
                finalFOV, CameraFOVSettings.FOVSmoothTime, _clampedDeltaTime) : targetFOV;
         
        }
        
        #region Private Methods

        private float UpdateFinalFOV(float baseFOV)
        {
            if(!CameraFOVSettings.enabled || CameraFOVSettings == null) return baseFOV;
            
            var movementSm = _moveState.UpdateMoveState();

            var targetSpeedMultiplier = ActiveState switch
            {
                RunState => CameraFOVSettings.fovRunStateCurve.Evaluate(_currentSpeed),
                IdleState or WalkState or null => 1f,
                _ => FOVSpeedMultiplier
            };
            
            FOVSpeedMultiplier = InterpolationHelperFunctions.ValueInterpolation(
                FOVSpeedMultiplier,
                targetSpeedMultiplier, 
                CameraFOVSettings.SmoothingFactor, 
                _clampedDeltaTime
                );
            
            _previousCharacterDirection = InterpolationHelperFunctions.ValueInterpolation(
                _previousCharacterDirection, 
                ForwardDirection, 
                CameraFOVSettings.FOVSmoothTime, 
                _clampedDeltaTime
                );
            
            var fovStopCurves = _previousCharacterDirection > 0.1f 
                ? CameraFOVSettings.fovForwardStopCurve.Evaluate(_currentSpeed) 
                : CameraFOVSettings.fovBackwardStopCurve.Evaluate(_currentSpeed);
            
            var targetMoveStateMultiplier = movementSm switch
            {
                MovementStateSM.MoveState.Stop when Mathf.Abs(_previousCharacterDirection) > 0.001f => 
                    fovStopCurves,
                _ => 1f
            };
            
            FOVStateMultiplier = InterpolationHelperFunctions.ValueInterpolation(
                FOVStateMultiplier,
                targetMoveStateMultiplier, 
                CameraFOVSettings.SmoothingFactor, 
                _clampedDeltaTime
                );
            
            SlopeFOVCalculation();

            var targetFOV = FOVSpeedMultiplier * FOVStateMultiplier * FOVSlopeScalingMultiplier;
            targetFOV = Mathf.Clamp(targetFOV, 0.5f, 2.0f);

            return baseFOV * targetFOV;
        }

        private void SlopeFOVCalculation()
        {
            var absoluteFactor = Mathf.Abs(_playerSlopeFactor.Factor);
            var factorSign = Mathf.Sign(_playerSlopeFactor.Factor);
            
            var downHillSlopeCurve = CameraFOVSettings.fovDownhillSlopeStateCurve.Evaluate(absoluteFactor);
            var upHillSlopeCurve =  CameraFOVSettings.fovUphillSlopeStateCurve.Evaluate(absoluteFactor);

            var targetCurve = factorSign switch
            {
                < 0.15f => upHillSlopeCurve,
                > 0.15f => downHillSlopeCurve,
                _ => 1f
            };
            
            FOVSlopeScalingMultiplier = InterpolationHelperFunctions.ValueInterpolation(
                FOVSlopeScalingMultiplier, 
                targetCurve, 
                CameraFOVSettings.FOVSmoothSlopeCurveTime, 
                _clampedDeltaTime
                );
        }
        #endregion
        

    }
}
