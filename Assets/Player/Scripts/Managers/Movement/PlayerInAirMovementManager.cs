using System;
using Cysharp.Threading.Tasks;
using Player.Interfaces.Movement.Context;
using Player.Managers.Interfaces;
using Player.Scripts.PlayerSettings.Movement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Managers.Movement
{
    public class PlayerInAirMovementManager : IAirMovement
    {
        #region Interface Implementation
        private static bool FoundAnyGround => PlayerContext.Movement.FoundAnyGround;
        private static Vector3 CharacterUp => PlayerContext.Orientation.CharacterUp;
        private static Vector3 GroundNormal => PlayerContext.Movement.GroundNormal;
        private static float ClampedDeltaTime => PlayerContext.State.ClampedDeltaTime;
        public InAirMovementSettings Settings { get; set; }
        #endregion
        
        #region Air Movement Parameters
        private float _airAccelerationSpeed;
        private float _maxAirMoveSpeed;
        #endregion

        public async UniTask<InAirMovementSettings> Initialize()
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<InAirMovementSettings>("AirMovementSettings");
                
                Settings = await handle.ToUniTask();
                
                _airAccelerationSpeed = Settings.AirAccelerationSpeed;
                _maxAirMoveSpeed = Settings.MaxAirMoveSpeed;

                return Settings;
            }
            catch(Exception e)
            {
                Debug.LogError("Failed to load CameraRotationSettings: " + e.Message);
                throw;
            }
        }

        #region Air Movement

        /// <summary>
        /// Handles acceleration while the character is airborne.
        /// Includes velocity clamping and wall climb prevention.
        /// </summary>
        public void HandleInAirMovement(
            ref Vector3 velocity,
            Vector3 moveInputVector)
        {
            // No input means no acceleration
            if (moveInputVector.sqrMagnitude <= 0f)
                return;

            // Compute air acceleration
            var addedVelocity = moveInputVector * (_airAccelerationSpeed * ClampedDeltaTime);

            // Project velocity onto horizontal movement plane
            var velocityOnInputPlane = Vector3.ProjectOnPlane(velocity, CharacterUp);

            /*
             Limit total air speed caused by kccMovement input.
            */
            if (velocityOnInputPlane.magnitude < _maxAirMoveSpeed)
            {
                var newTotal = Vector3.ClampMagnitude(
                    velocityOnInputPlane + addedVelocity,
                    _maxAirMoveSpeed);

                addedVelocity = newTotal - velocityOnInputPlane;
            }
            else
            {
                /*
                 If already exceeding max air speed,
                 prevent acceleration in the same direction.
                */
                if (Vector3.Dot(velocityOnInputPlane, addedVelocity) > 0f)
                {
                    addedVelocity = Vector3.ProjectOnPlane(
                        addedVelocity,
                        velocityOnInputPlane.normalized);
                }
            }

            /*
             Prevent climbing steep surfaces while airborne.
            */
            if (FoundAnyGround)
            {
                if (Vector3.Dot(velocity + addedVelocity, addedVelocity) > 0f)
                {
                    var obstructionNormal =
                        Vector3.Cross(
                            Vector3.Cross(CharacterUp, GroundNormal),
                            CharacterUp).normalized;

                    addedVelocity = Vector3.ProjectOnPlane(
                        addedVelocity,
                        obstructionNormal);
                }
            }

            // Apply final air acceleration
            velocity += addedVelocity;
        }

        #endregion
    }
}
