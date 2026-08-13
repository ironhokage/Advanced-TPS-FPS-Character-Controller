using System;
using Player.Interfaces.Movement.Context;
using Player.PlayerSettings.Managers;
using Player.Spring;
using Player.StateMachines.CameraSM_s;
using UnityEngine;

namespace Player.Managers.Helpers
{
    public class HeadBobManager
    {
        #region Enums

        private enum MoveState
        {
            Idle,
            Start,
            Stop,
            Moving
        }
        
        #endregion
        #region Interface Implementation
        private static float InputMagnitude => GroundMovementContext.InputHooks != null ? GroundMovementContext.InputHooks.InputMagnitude : 0f;
        public float SpeedRatio => GroundMovementContext.MovementHooks != null ? GroundMovementContext.MovementHooks.SpeedRatio : 0f;
        private static float LocalForwardVelocity => GroundMovementContext.MovementHooks != null ? GroundMovementContext.MovementHooks.LocalForwardVelocity : 0f;
        #endregion

        #region Fields
        
        private FloatSpring _horiSpring;
        private FloatSpring _vertSpring;

        private bool _activateSystem;
        
        private float _horiSpringValue;
        private float _vertSpringValue;
        
        private float _horiValueSign;
        private float _vertValueSign;
        
        private MoveState _moveState;
        private float _movingDuration;
        
        private readonly MovementStateSM _moveStateMachine = new();
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Calculates the directional lean/tilt overlay quaternion based on movement.
        /// </summary>
        public Vector3 HeadBobDirectionModifier(HeadBobManagerSettings settings)
        {
            if (settings == null || !settings.EnableEffect || _horiSpring == null || _vertSpring == null) 
                return Vector3.zero;
            
            var charDirection = InputMagnitude;
            var charSpeed = SpeedRatio;
            
            if (charDirection > 0.001f)
            {
                _horiSpringValue = LocalForwardVelocity;
            }
            
            UpdateCharMoveState(settings);
            
            var horiSpringValue = _horiSpring.Evaluate(Time.deltaTime);
            var vertSpringValue = _vertSpring.Evaluate(Time.deltaTime);
            
            var vectorSpring = new Vector3(horiSpringValue, vertSpringValue, 0f);
            
            _horiSpring.CurrentValue = Mathf.Clamp(_horiSpring.CurrentValue, settings.MinLimit, settings.MaxLimit);
            
            return vectorSpring;
        }

        private void UpdateCharMoveState(HeadBobManagerSettings settings)
        {
            var signedHoriValue = _horiSpringValue * _horiValueSign * settings.HoriStrength;
            var signedVertValue = _vertSpringValue * _horiValueSign * settings.VertStrength;
            
            switch (_moveState)
            {
                case MoveState.Start:
                    _movingDuration = 0f;
                    break;
                case MoveState.Stop:
                    if (_movingDuration > 0.1f)
                    {
                       // _horiSpring.UpdateEndValue(0f, signedHoriValue);
                       // _vertSpring.UpdateEndValue(0f, signedVertValue);
                    }
                    break;

                case MoveState.Idle:
                    _movingDuration = 0f;
                    break;
                case MoveState.Moving:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Recreates the spring with current settings. (Called each frame from CameraBrain.Update)
        /// </summary>
        public void SetInitialValues(HeadBobManagerSettings settings)
        {
            if (settings == null) return;

            _horiSpring = new FloatSpring
            {
                StartValue = 0f,
                Damping = settings.HoriDamping,
                Stiffness = settings.HoriStiffness
            };

            _vertSpring = new FloatSpring
            {
                StartValue = 0f,
                Damping = settings.VertDamping,
                Stiffness = settings.VertStiffness
            };
        }

        /// <summary>
        /// Allows live update of spring parameters from the inspector.
        /// </summary>
        public void CheckInspectorChanges(HeadBobManagerSettings settings)
        {
            if (_horiSpring == null || _vertSpring == null || settings == null) return;

            _horiSpring.Damping = settings.HoriDamping;
            _horiSpring.Stiffness = settings.HoriStiffness;
            
            _vertSpring.Damping = settings.VertDamping;
            _vertSpring.Stiffness = settings.VertStiffness;
        }
        #endregion
    }
}
