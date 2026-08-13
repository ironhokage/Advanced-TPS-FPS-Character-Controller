using Cysharp.Threading.Tasks;
using Player.Camera.SOs;
using UnityEngine;

namespace Player.Camera.Interfaces.CameraCore
{
    public interface ICameraPosition
    {
        UniTask<CameraPositionSettings> InstantiateAsync();

        void ComputeCameraPositionCoordinates(bool isInFPS, GameObject camHolder, Transform characterTransform, Vector2 lookVector, float pitch);
        void SetCameraAxes(float pitch, float yaw);
        CameraPositionSettings PositionSettings { get; set; }
        Vector3 CharUp { get; set; }
    }
}
