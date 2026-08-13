using Player.PlayerSettings.Camera;
using UnityEngine;

namespace Player.Input.Interface
{
    public interface IMouse
    {
        Vector2 MouseInput(Vector2 input, CameraInputSettings inputSettings);
    }
}