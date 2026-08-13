using System;
using Cysharp.Threading.Tasks;
using Player.Camera.SOs;
using Player.Scripts.Camera.Interfaces.CameraCore;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Scripts.Camera.Core
{
    public class CameraPositionManager : ICameraPosition
    {
        public CameraPositionSettings PositionSettings { get; set; }
        public Vector3 CharUp { get; set; }

        public async UniTask<CameraPositionSettings> InstantiateAsync()
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<CameraPositionSettings>("CameraPositionSettings");
                
                PositionSettings = await handle.ToUniTask();
                
                return PositionSettings;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public void ComputeCameraPositionCoordinates(bool isInFPS, GameObject cam, Transform characterTransform, float smoothedYaw, float smoothedPitch)
        {
            if (cam == null) return;
            if (isInFPS)
            {
                CalculateFPSPosition(cam, characterTransform);
            }
            else
            {
                CalculateTPSPosition(cam, characterTransform, smoothedYaw, smoothedPitch);
            }
        }

        private void CalculateFPSPosition(GameObject cam,Transform characterTransform)
        {
            var eyeOffset = PositionSettings.FPSEyeOffset * CharUp ;
            
            var forwardDir = Vector3.ProjectOnPlane(characterTransform.forward, CharUp).normalized;
            
            var forwardOffset = forwardDir * PositionSettings.ForwardDistance;
            cam.transform.position = characterTransform.position + eyeOffset + forwardOffset;
        }

        private void CalculateTPSPosition(GameObject cam, Transform characterTransform, float smoothedYaw, float smoothedPitch)
        {
            var fullRotation = Quaternion.Euler(smoothedPitch, smoothedYaw, 0f);
            
            var localOffset = new Vector3(
                PositionSettings.TPSXAxisOffset,
                PositionSettings.TPSEyeOffset,
                PositionSettings.TPSZAxisOffset);
            
            var worldOffset = fullRotation * localOffset;
    
            var lookTarget = characterTransform.position + CharUp * PositionSettings.TPSEyeOffset;
            
            cam.transform.position = characterTransform.position + worldOffset;
            
            cam.transform.LookAt(lookTarget, CharUp);
        }


    }
}

