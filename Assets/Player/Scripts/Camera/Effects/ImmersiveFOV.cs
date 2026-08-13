using Player.Camera.Statics;
using Player.Interfaces.Camera.CameraEffects;
using Player.PlayerSettings.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Camera.Effects
{
    public class ImmersiveFOV : IBodyImmersiveFOV
    {
        public ImmersiveFOVSettings ImmersiveFOVSettings { get; set; }
        public float PitchFactor { get; set; }
        
        private CinemachineCamera _vCam;
        private Transform _cameraTransform;
        private Vector3 _activeOffset;

        public void Initialize(Transform cameraTransform, CinemachineCamera vCam)
        {
            _cameraTransform = cameraTransform;
            _vCam = vCam;
        }

        public Vector3 DepthOffsetFOVModifier(float cameraPitch, Vector3 anchorBasePosition, float deltaTime)
        {
            if (!ImmersiveFOVSettings || !_vCam || !_cameraTransform) return anchorBasePosition;

            PitchFactor = Mathf.InverseLerp(ImmersiveFOVSettings.startAngle, ImmersiveFOVSettings.endAngle, cameraPitch);
            var localOffset = new Vector3(0, ImmersiveFOVSettings.MaxUpOffset, ImmersiveFOVSettings.MaxBackOffset) * PitchFactor;
            
            var parentTransform = _cameraTransform.parent ? _cameraTransform.parent : _cameraTransform;
            var worldOffsetGoal = parentTransform.TransformDirection(localOffset);
            
            _activeOffset = Vector3.Lerp(
                _activeOffset, 
                worldOffsetGoal, 
                InterpolationHelperFunctions.ExpDecayValue(ImmersiveFOVSettings.SmoothingFactor, deltaTime)
            );
            
            return anchorBasePosition + _activeOffset;
        }
    }
}
