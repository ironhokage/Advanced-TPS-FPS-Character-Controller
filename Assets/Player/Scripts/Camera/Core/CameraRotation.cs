using System;
using Cysharp.Threading.Tasks;
using Player.Camera.Interfaces.CameraCore;
using Player.Camera.SOs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Scripts.Camera.Core
{
    public class CameraRotation : ICameraRotation
    {
        #region Fields
        public CameraRotationSettings RotationSettings;
        #endregion

        private float _targetYaw;
        private float _fpsPitch;
        private float _tpsPitch;
        
        public float SmoothedYaw;
        public float SmoothedPitch;
        
        public Quaternion cameraRotation { get; set; }

        public async UniTask<CameraRotationSettings> InstantiateAsync()
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<CameraRotationSettings>("CameraRotationSettings");
                
                RotationSettings = await handle.ToUniTask();

                return RotationSettings;
            }
            catch(Exception e)
            {
                Debug.LogError("Failed to load CameraRotationSettings: " + e.Message);
                throw;
            }
        }

        #region Public Methods
        public void HandleRotationInput(Vector2 mousePosition)
        {
            if (RotationSettings == null)
                return;
            
            if(mousePosition.magnitude < 0.01f) return;

            _targetYaw += mousePosition.x * 0.75f;
            
            _fpsPitch = CalculateSoftFPSPitch(ref _fpsPitch, mousePosition.y);
            _tpsPitch = CalculateTPSPitch(ref _tpsPitch, mousePosition.y);
        }

        public float CameraUpdateRotation(GameObject cameraObject, bool isInFPS)
        {
            if (RotationSettings == null || cameraObject == null)
                return 0f;

            cameraRotation = Rotation(isInFPS);
            cameraObject.transform.rotation = cameraRotation;

            return isInFPS ? SmoothedYaw : 0.0f;
        }
        #endregion
        
        public void ResetTpsOrientation(float yaw, float pitch)
        {
            _targetYaw = yaw;
            SmoothedYaw = yaw;
            _tpsPitch = pitch;
            SmoothedPitch = pitch;
        }

        #region Private Methods
        
        
        private Quaternion Rotation(bool isInFPS)
        {
            var delta = Time.deltaTime;
            
            var weightX = 1.0f - Mathf.Exp(-RotationSettings.SmoothingFactorX * delta);
            var weightY = 1.0f - Mathf.Exp(-RotationSettings.SmoothingFactorY * delta);

            var finalPitch = isInFPS ? _fpsPitch : _tpsPitch;
            
            SmoothedYaw = Mathf.Lerp(SmoothedYaw, _targetYaw, weightX);
            SmoothedPitch = Mathf.Lerp(SmoothedPitch, finalPitch, weightY);
            
            return Quaternion.Euler(SmoothedPitch, SmoothedYaw, 0f);
        }

        private float CalculateSoftFPSPitch(ref float currentPitch, float deltaPitch)
        {
            var limitMin = Mathf.Min(RotationSettings.FPSMaxUpPitch, RotationSettings.FPSMaxDownPitch); 
            var limitMax = Mathf.Max(RotationSettings.FPSMaxUpPitch, RotationSettings.FPSMaxDownPitch); 
            var softZone = RotationSettings.SoftZoneDegrees;

            var resistanceFactor = 1.0f;

            switch (deltaPitch)
            {
                case > 0:
                {
                    var threshold = limitMin + softZone;
                    if (currentPitch < threshold)
                    {
                        var distance = Mathf.Clamp01((threshold - currentPitch) / softZone);
                        resistanceFactor = Mathf.Pow(1.0f - distance, 2.0f);
                    }
                    break;
                }
               
                case < 0:
                {
                    var threshold = limitMax - softZone;
                    if (currentPitch > threshold)
                    {
                        var distance = Mathf.Clamp01((currentPitch - threshold) / softZone);
                        resistanceFactor = Mathf.Pow(1.0f - distance, 2.0f);
                    }
                    break;
                }
            }

            currentPitch -= deltaPitch * resistanceFactor;
            
            return Mathf.Clamp(currentPitch, limitMin, limitMax);
        }

        private float CalculateTPSPitch(ref float currentPitch, float deltaPitch)
        {
            var limitMin = Mathf.Min(RotationSettings.TPSMaxUpPitch, RotationSettings.TPSMaxDownPitch); 
            var limitMax = Mathf.Max(RotationSettings.TPSMaxUpPitch, RotationSettings.TPSMaxDownPitch); 
            var softZone = RotationSettings.SoftZoneDegrees;

            var resistanceFactor = 1.0f;

            switch (deltaPitch)
            {
                case > 0:
                {
                    var threshold = limitMin + softZone;
                    if (currentPitch < threshold)
                    {
                        var distance = Mathf.Clamp01((threshold - currentPitch) / softZone);
                        resistanceFactor = Mathf.Pow(1.0f - distance, 2.0f);
                    }
                    break;
                }
               
                case < 0:
                {
                    var threshold = limitMax - softZone;
                    if (currentPitch > threshold)
                    {
                        var distance = Mathf.Clamp01((currentPitch - threshold) / softZone);
                        resistanceFactor = Mathf.Pow(1.0f - distance, 2.0f);
                    }
                    break;
                }
            }

            currentPitch -= deltaPitch * resistanceFactor;
            
            return Mathf.Clamp(currentPitch, limitMin, limitMax);
        }
        
        #endregion
    }
}