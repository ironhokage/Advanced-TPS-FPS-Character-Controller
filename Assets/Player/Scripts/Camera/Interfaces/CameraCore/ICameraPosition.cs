using Cysharp.Threading.Tasks;
using Player.Camera.SOs;
using UnityEngine;

namespace Player.Scripts.Camera.Interfaces.CameraCore
{
    public interface ICameraPosition
    {
        UniTask<CameraPositionSettings> InstantiateAsync();

        void ComputeCameraPositionCoordinates(bool isInFPS, GameObject cam, Transform characterTransform, float smoothedYaw, float smoothedPitch);
        CameraPositionSettings PositionSettings { get; set; }
        Vector3 CharUp { get; set; }
    }
}
