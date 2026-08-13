using Player.PlayerSettings.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Interfaces.Camera.CameraEffects
{
    public interface IBodyImmersiveFOV
    {
        ImmersiveFOVSettings ImmersiveFOVSettings { get; set; }
        Vector3 DepthOffsetFOVModifier(float cameraPitch, Vector3 anchorBasePosition, float deltaTime);
        void Initialize(Transform cameraTransform, CinemachineCamera vCam);
        float PitchFactor  { get; set; }

    }
}
