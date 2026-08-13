using System;
using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.Interfaces.Movement;
using Player.Interfaces.Movement.Context;
using Player.Managers.Helpers;
using Player.Managers.Interfaces;
using Player.Managers.Locomotion.SubMovementManagers;
using Player.PlayerSettings.Movement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Managers.Movement
{
    public class PlayerGroundMovementManager : IGroundMovementAnimationHooks, IGroundMovementDirectionHooks, IGroundMovementInputHooks, IGroundMovement
    {
        #region Interface Implementation
        public Vector3 ForwardBody => PlayerContext.Orientation.ForwardBody;
        public Vector3 RightBody => PlayerContext.Orientation.RightBody;
        private static Vector3 CharacterUp => PlayerContext.Orientation.CharacterUp;
        private static Vector3 GroundNormal => PlayerContext.Movement.GroundNormal;
        private static float ClampedDeltaTime => PlayerContext.State.ClampedDeltaTime;
        private static KinematicCharacterMotor Motor => PlayerContext.State.Motor;
        public MovementSettings Settings { get; set; }
        #endregion

        #region Frame State
        private float _speedRatio;
        private PlayerGroundMovementHelper.SlopeData _slopeModifierData;
        #endregion
        
        #region Movement Methods
        private Vector3 _currentMoveVector;
        private Vector3 _moveDirectionNormalized;
        #endregion
        
        #region Input Methods
        private float _inputMagnitude;
        private float _inputResponse;
        private Vector3 _reorientedInput;
        private Vector3 _smoothedReorientedInput;
        private Vector3 _inputDirection;
        private Vector3 _smoothedInputDirection;
        private Vector3 _inputNorm;
        #endregion
        
        #region Direction Variables
        private float _turn;
        private float _alignment;
        private float _forwardDot;
        private float _rightDot;
        private float _localForwardVelocity;
        private float _localRightVelocity;
        private Vector3 _movingPreviousDirection;
        private Vector3 _previousVelocityDirection;
        #endregion
        
        #region Sub-Managers
        private readonly PlayerSpeedManager _playerSpeedManager = new();
        private readonly PlayerGroundMovementHelper _playerGroundMovementHelper = new();
        private readonly PlayerResponsivenessManager _playerResponsivenessManager = new();
        #endregion

        #region Initialization
        
        public async UniTask<MovementSettings> Initialize()
        {
            _movingPreviousDirection = ForwardBody;
            _previousVelocityDirection = ForwardBody;
            
            try
            {
                var handle = Addressables.LoadAssetAsync<MovementSettings>("MovementSettings");
                
                Settings = await handle.ToUniTask();
                
                SetupInterfaces();
                return Settings;
            }
            catch(Exception e)
            {
                Debug.LogError("Failed to load CameraRotationSettings: " + e.Message);
                throw;
            }
        }

        #endregion

        private void SetupInterfaces()
        {
            GroundMovementContext.DirectionHooks = this;
            GroundMovementContext.InputHooks = this;
            GroundMovementContext.MovementHooks = this;
        }

        #region Main Entry
        public void HandleGroundMovement(ref Vector3 velocity, Vector3 moveInputVector)
        {
            
            _speedRatio = velocity.magnitude / Settings.MaxMovementSpeed;
           
            UpdateInputState(moveInputVector);
            
            ApplyMovement(ref velocity);

            if (GroundNormal.sqrMagnitude > 0.001f)
            {
                velocity = Motor.GetDirectionTangentToSurface(velocity, GroundNormal) * velocity.magnitude;
            }
        }
        #endregion
        
        #region Private Methods

        private void UpdateInputState(Vector3 moveInputVector)
        {
            var hasValidGround = GroundNormal.sqrMagnitude > 0.001f;

            _reorientedInput = moveInputVector.sqrMagnitude > 0.0001f && hasValidGround
                ? Vector3.ProjectOnPlane(moveInputVector,  GroundNormal)
                : Vector3.zero;
            
            _currentMoveVector = Vector3.Lerp(_currentMoveVector, moveInputVector, 1f - Mathf.Exp(-Settings.SmoothedMovement * ClampedDeltaTime));
            
            if(_currentMoveVector.sqrMagnitude > 0.0001f && hasValidGround)
            {
                _smoothedReorientedInput = Vector3.ProjectOnPlane(_currentMoveVector,  GroundNormal);
            }
        }

        private void ApplyMovement(ref Vector3 velocity)
        {
            InputCalculations();

            AlignmentCalculations();
            
            _slopeModifierData = _playerGroundMovementHelper.CalculateSlope(
                Settings.MaxSlopeAngle, 
                Settings, velocity);
            
            // Determine target speed based on movement settings, slope, etc.
           var targetSpeed = _playerSpeedManager.ApplyDynamicSpeed(
                Settings,
                _slopeModifierData,
                _forwardDot,
                _playerGroundMovementHelper
            );
            
            // Compute responsiveness factoring in slope, speed, alignment
            var responsiveness = _playerResponsivenessManager.ApplyDynamicResponsiveness(
                _smoothedInputDirection,
                _slopeModifierData,
                _speedRatio,
                Settings,
                _forwardDot,
                _alignment,
                _playerGroundMovementHelper
            );
            
            var dirT = (1f + _alignment) / 2f;
            
            var startBoostFactor = Settings.startBoostCurve.Evaluate(_speedRatio);
            
            var turnBoostFactor = Settings.turnBoostCurve.Evaluate(dirT);
            
            responsiveness *= turnBoostFactor;
            
            targetSpeed *= startBoostFactor;
            
            var accelLerp = (1f - Mathf.Exp(-responsiveness * Settings.AccelerationSmoothingFactor * ClampedDeltaTime)) * _inputResponse;
            
            var speedWeight = Settings.speedWeightCurve.Evaluate(dirT);
            var targetVelocity = _smoothedInputDirection * targetSpeed * _inputMagnitude * speedWeight;
            
            velocity = Vector3.Lerp(velocity, targetVelocity, accelLerp);
            
            _moveDirectionNormalized= velocity.sqrMagnitude > 0.001f 
                ? velocity.normalized 
                : _smoothedInputDirection.normalized;
            
            TurnCalculations(velocity);
            
            // Apply drag when no input
            var dragLerp = 1f - Mathf.Exp(-Settings.DragValue * ClampedDeltaTime);
            var dragWeight = 1f - _inputResponse;
            
            velocity = Vector3.Lerp(velocity, Vector3.zero, dragLerp * dragWeight);

            if (_inputMagnitude > 0.001f)
            {
                _movingPreviousDirection = Vector3.Lerp(_movingPreviousDirection, _inputNorm,
                    Settings.directionalBlendCurve.Evaluate(Mathf.Clamp(dirT, 0.1f, 1f)) * _inputMagnitude);
            }

            var guardedVelocity = velocity.sqrMagnitude > 0.0001f ? velocity.normalized : _previousVelocityDirection;
            
            _previousVelocityDirection = Vector3.Lerp(_previousVelocityDirection, guardedVelocity,
                Settings.directionalBlendCurve.Evaluate(Mathf.Clamp(dirT, 0.1f, 1f)) * _speedRatio);
            
            var speedWeightTransition = 1f - Mathf.Exp(-Settings.SoftClampPower * (_speedRatio - 1f) * ClampedDeltaTime);
            var newVelocity = velocity.normalized * Settings.MaxMovementSpeed;
            
            velocity = Vector3.Lerp(velocity, newVelocity, speedWeightTransition);
        }
        #endregion

        #region HelperMethods
        private void InputCalculations()
        {
            // Compute input magnitude and normalized direction
            _inputMagnitude = Mathf.Clamp01(_reorientedInput.magnitude);
            _inputResponse = Settings.inputResponseCurve.Evaluate(_inputMagnitude);
            
            _inputDirection = _inputMagnitude > 0.001f ? _reorientedInput : Vector3.zero;
            _smoothedInputDirection = _inputMagnitude > 0.001f ? _smoothedReorientedInput : Vector3.zero;
            
            _inputNorm = _smoothedInputDirection.sqrMagnitude > 0.0001f
                ? _smoothedInputDirection.normalized
                : ForwardBody;
        }

        private void AlignmentCalculations()
        {
            _forwardDot = Vector3.Dot(_inputNorm, ForwardBody);
            _rightDot = Vector3.Dot(_inputNorm, ForwardBody);
            
            _localForwardVelocity = Vector3.Dot(_inputDirection.normalized, ForwardBody);
            _localRightVelocity = Vector3.Dot(_inputDirection.normalized, RightBody);

            _alignment = _inputDirection.sqrMagnitude > 0.001f
                ? Vector3.Dot(_movingPreviousDirection, _inputNorm) : 0f;
        }

        private void TurnCalculations(Vector3 velocity)
        {
            var currentDir = velocity.sqrMagnitude > 0.001f 
                ? velocity.normalized 
                : _previousVelocityDirection;

            var angleDelta = Vector3.SignedAngle(_previousVelocityDirection, currentDir, CharacterUp);

            var turnRate = angleDelta / 180f;
            
            _turn = Mathf.Lerp(_turn, turnRate, 1f - Mathf.Exp(-10f * ClampedDeltaTime));
        }
        #endregion

        #region Interface Implementation
        public float SpeedRatio => _speedRatio;
        public float ForwardDot => _forwardDot;
        public float RightDot => _rightDot;
        public float LocalForwardVelocity => _localForwardVelocity;
        public float LocalRightVelocity => _localRightVelocity;
        public float Alignment => _alignment;
        public float TurnRate => _turn;
        public float InputMagnitude => _inputMagnitude;
        public Vector3 InputDirection => _inputDirection;
        public Vector3 SmoothedInputDirection => _smoothedInputDirection;
        public Vector3 MoveDirection => _moveDirectionNormalized;
        public PlayerGroundMovementHelper.SlopeData SlopeData => _slopeModifierData;
        #endregion
    }
}