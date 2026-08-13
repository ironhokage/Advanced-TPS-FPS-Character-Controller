using Cysharp.Threading.Tasks;
using Player.PlayerSettings.Camera;
using UnityEngine;

namespace Player.Camera.Interfaces.CameraCore
{
    public interface ICameraInput
    {
        Vector2 FinalInput { get; set; }
        UniTask<CameraInputSettings> InstantiateAsync();
        Vector2 InputHandler();
    }
}
