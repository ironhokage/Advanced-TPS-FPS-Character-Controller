using Player.Input.Interface;
using Player.PlayerSettings.Camera;
using UnityEngine;

namespace Player.Camera.Core
{
    public class MouseInputHandler : IMouse
    {
        private const float DeltaSmoothFactor = 0.12f;   
        private const float ReferenceMagnitude = 30f;    
        
        private Vector2 _smoothedDelta = Vector2.zero;

        public Vector2 MouseInput(Vector2 input, CameraInputSettings inputSettings)
        {
            _smoothedDelta = Vector2.Lerp(_smoothedDelta, input, DeltaSmoothFactor);
            
            var magnitude = _smoothedDelta.magnitude;
            
            var curveInput = magnitude / ReferenceMagnitude; 
            var curveValue = inputSettings.smoothingCurve.Evaluate(curveInput);
            
            var curvedDelta = _smoothedDelta * curveValue;
            
            var finalX = curvedDelta.x * inputSettings.xSensibility;
            var finalY = curvedDelta.y * inputSettings.ySensibility;

            if (inputSettings.invertX) finalX *= -1;
            if (inputSettings.invertY) finalY *= -1;

            return new Vector2(finalX, finalY);
        }
    }
}