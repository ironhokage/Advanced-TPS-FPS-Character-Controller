using Player.Interfaces.Input;
using Player.PlayerSettings.Camera;
using UnityEngine;

namespace Player.Camera.Core
{
    public class JoystickInputHandler : IJoystick
    {
        private const float MaxAngularSpeedX = 180f;
        private const float MaxAngularSpeedY = 360f;
        
        public Vector2 JoystickInput(Vector2 input, CameraInputSettings inputSettings)
        {
            float inputStrength;
            Vector2 inputDirection;
            
            if (input.magnitude > 0.25f)
            {
                inputDirection = input.normalized;
                inputStrength = input.magnitude;
            }
            else
            {
                inputDirection = Vector2.zero;
                inputStrength = 0.0f;
            }
            
            var angularDeltaX = inputDirection.x * MaxAngularSpeedX * Time.deltaTime * inputStrength;
            var angularDeltaY = inputDirection.y * MaxAngularSpeedY * Time.deltaTime * inputStrength;
            
            var xAxis = angularDeltaX * inputSettings.xJoySensibility;
            var yAxis = angularDeltaY * inputSettings.yJoySensibility;
            
            var maxPossibleX = MaxAngularSpeedX * Time.deltaTime * inputSettings.xJoySensibility;
            var maxPossibleY = MaxAngularSpeedY * Time.deltaTime * inputSettings.yJoySensibility;
            
            var combinedMagnitude = new Vector2(xAxis, yAxis).magnitude;
            var maxCombinedMagnitude = new Vector2(maxPossibleX, maxPossibleY).magnitude;
            var normalizedMagnitude = Mathf.Clamp01(combinedMagnitude / maxCombinedMagnitude);
            
            var curve = inputSettings.smoothingJoystickCurve.Evaluate(normalizedMagnitude);

            var finalX = xAxis * curve;
            var finalY = yAxis * curve;

            if (inputSettings.invertX) finalX *= -1;
            if (inputSettings.invertY) finalY *= -1;

            return new Vector2(finalX, finalY);
        }
    }
}