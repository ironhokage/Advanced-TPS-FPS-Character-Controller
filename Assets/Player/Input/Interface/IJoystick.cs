using Player.PlayerSettings.Camera;
using UnityEngine;

namespace Player.Interfaces.Input
{
    public interface IJoystick
    {
        Vector2 JoystickInput(Vector2 input, CameraInputSettings inputSettings);
    }
}