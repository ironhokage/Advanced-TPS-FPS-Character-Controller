using Cysharp.Threading.Tasks;
using Player.Camera.SOs;
using UnityEngine;

namespace Player.Camera.Interfaces.CameraCore
{
    public interface ICameraRotation
    {
        void HandleRotationInput(Vector2 mousePosition);
        float CameraUpdateRotation(GameObject cameraObject, bool isInFPS);
        UniTask<CameraRotationSettings> InstantiateAsync();

        Quaternion cameraRotation { get; set; }
        
    }
}
