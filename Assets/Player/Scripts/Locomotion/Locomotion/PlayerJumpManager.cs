using System;
using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.Managers.Interfaces;
using Player.PlayerSettings.Jump;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Locomotion.Locomotion
{
    public class PlayerJumpManager : IJump
    {
        #region Fields
        public JumpSettings Settings { get; set; }
        public KinematicCharacterMotor Motor { get; set; }
        
        public bool IsJumpActive;
        private float _velocityTakeoff;
        
        private float _jumpBufferCounter;
        private IJump _jumpImplementation;
        public bool CanJump;
        #endregion

        #region Initialization
        public async UniTask<JumpSettings> InitializeJumpSett()
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<JumpSettings>("JumpSettings");
                
                Settings = await handle.ToUniTask();
                IsJumpActive = false;
                
                return Settings;
            }
            catch (Exception e)
            {
               Debug.LogException(e);
               throw;
            }
        }
        
        #endregion

        #region Input Handlers

        public void HandleJumpPressed()
        {
            if(!CanJump) return;
            
            if (Motor ==null)
            {
                Debug.LogError($"{nameof(PlayerJumpManager)}.{nameof(HandleJumpPressed)}: Motor is null");
                return;
            }
            
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                IsJumpActive = true;
                _velocityTakeoff = Mathf.Sqrt(2f * Mathf.Abs(Settings.JumpUpSpeed) * Settings.JumpHeight);
            }
            else
            {
                _jumpBufferCounter = Settings.JumpBufferTime;
            }
        }

        public void HandleJumpReleased() { }

        #endregion

        #region Physics & Update Logic

        public void ApplyJumpForce(ref Vector3 currentVelocity)
        {
            if (!IsJumpActive) return;

            Motor.ForceUnground(0f);

            var currentVerticalSpeed = Vector3.Dot(currentVelocity, Motor.CharacterUp);
            var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, _velocityTakeoff);
            var jumpVelocityDifference = Motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
            currentVelocity += jumpVelocityDifference;

            IsJumpActive = false;
            _jumpBufferCounter = 0f;
        }

        #endregion

        #region Timers
        public void UpdateJumpBuffer(float delta)
        {
            _jumpBufferCounter = _jumpBufferCounter >= 0f ? Mathf.Max(0f, _jumpBufferCounter - delta) : 0f;
            if (_jumpBufferCounter > 0f && Motor.GroundingStatus.IsStableOnGround)
            {
                IsJumpActive = true;
                _velocityTakeoff = Mathf.Sqrt(2f * Mathf.Abs(Settings.JumpUpSpeed) * Settings.JumpHeight);
            }
        }

        #endregion
    }
}