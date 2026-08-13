using System;
using Player.Interfaces.Movement.Context;
using Player.PlayerSettings.Managers;
using Player.Spring;
using Player.StateMachines.CameraSM_s;
using UnityEngine;

namespace Player.Managers.Helpers
{
    public class TiltLeanManager
    {
        #region Fields
        
        private FloatSpring _tiltSpring;
        
        private float _tiltValue;
        private float _leanValue;

        private Vector3 _charDirections;
        
        private float _tiltValueSign;

        private float _movingDuration;
        private readonly MovementStateSM _moveState = new();

        #endregion
        
        #region Interface Implementation
        private static float InputMagnitude => GroundMovementContext.InputHooks != null ? GroundMovementContext.InputHooks.InputMagnitude : 0f;
        private static float LocalForwardVelocity => GroundMovementContext.MovementHooks != null ? GroundMovementContext.MovementHooks.LocalForwardVelocity : 0f;
        private static float LocalRightVelocity => GroundMovementContext.MovementHooks != null ? GroundMovementContext.MovementHooks.LocalRightVelocity : 0f;
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Calculates the directional lean/tilt overlay quaternion based on movement.
        /// </summary>
        public Quaternion LeanDirectionModifier(TiltManagerSettings settings)
        {
            if (settings == null || !settings.EnableEffect || _tiltSpring == null) 
                return Quaternion.Euler(Vector3.zero);
            
            var charDirection = InputMagnitude;
            
            if (charDirection > 0.001f)
            {
                _tiltValue = LocalForwardVelocity;
                _leanValue = LocalRightVelocity;
                
                _charDirections = new Vector3(_tiltValue, 0f, _leanValue);
            }
            
            UpdateCharMoveState(settings);
            
            var tiltSpringValue = _tiltSpring.Evaluate(Time.deltaTime);
            
            var vectorSpring = new Vector3(tiltSpringValue, 0f, 0f);
            
            if (Mathf.Abs(_tiltSpring.CurrentValue) > settings.MaxLimit)
            {
                _tiltSpring.EndValue = 0f;
            }
            
            return Quaternion.Euler(vectorSpring);
        }

        private void UpdateCharMoveState(TiltManagerSettings settings)
        {
            var movementSm = _moveState.UpdateMoveState();

            if (movementSm == MovementStateSM.MoveState.Moving)
            {
                if (_movingDuration < 2f)
                    _movingDuration += Time.deltaTime;
            }

            _tiltValueSign = movementSm switch
            {
                MovementStateSM.MoveState.Start => -1f,
                MovementStateSM.MoveState.Stop => 1f,
                _ => _tiltValueSign
            };

            var signedTiltValue = _tiltValue * _tiltValueSign * settings.PerpendicularStrength;
            
            switch (movementSm)
            {
                case MovementStateSM.MoveState.Start:
                    _tiltSpring.UpdateEndValue(0f, signedTiltValue);
                    _movingDuration = 0f;
                    break;
                case MovementStateSM.MoveState.Stop:
                    if (_movingDuration > 0.1f)
                        _tiltSpring.UpdateEndValue(0f, signedTiltValue);
                    break;

                case MovementStateSM.MoveState.Idle:
                    _movingDuration = 0f;
                    break;
                case MovementStateSM.MoveState.Moving:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Recreates the spring with current settings. (Called each frame from CameraBrain.Update)
        /// </summary>
        public void SetInitialValues(TiltManagerSettings settings)
        {
            if (settings == null) return;

            _tiltSpring = new FloatSpring
            {
                StartValue = 0f,
                Damping = settings.TiltDamping,
                Stiffness = settings.TiltStiffness
            };
        }

        /// <summary>
        /// Allows live update of spring parameters from the inspector.
        /// </summary>
        public void CheckInspectorChanges(TiltManagerSettings settings)
        {
            if (_tiltSpring == null || settings == null) return;

            _tiltSpring.Damping = settings.TiltDamping;
            _tiltSpring.Stiffness = settings.TiltStiffness;
        }
        #endregion
    }
}
