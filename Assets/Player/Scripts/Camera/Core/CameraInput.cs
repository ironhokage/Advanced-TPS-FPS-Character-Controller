using System;
using Cysharp.Threading.Tasks;
using Player.Camera.Interfaces.CameraCore;
using Player.Input;
using Player.Input.Interface;
using Player.Interfaces.Input;
using Player.PlayerSettings.Camera;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Camera.Core
{
   
    public class CameraInput : ICameraInput
    {
        private CameraInputSettings _inputSettings;

        private readonly IMouse _mouse = new MouseInputHandler();
        private readonly IJoystick _joystick = new JoystickInputHandler();
        
        public Vector2 FinalInput { get; set; }

        public async UniTask<CameraInputSettings> InstantiateAsync()
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<CameraInputSettings>("CameraInputSettings");
                
                _inputSettings = await handle.ToUniTask();
                
                return _inputSettings;
            }
            catch (Exception exception)
            {
                Debug.LogError("Failed to load CameraInputSettings: " + exception.Message);
                throw;
            }
        }
        
        #region Input Handling
        
        public Vector2 InputHandler()
        {
            var initialInput = UniversalInputManager.Instance.CameraLookInput;
            
            var mouse = _mouse.MouseInput(initialInput, _inputSettings);
            var joystick = _joystick.JoystickInput(initialInput, _inputSettings);
            
            FinalInput = UniversalInputManager.CurrentInputType switch
            {
                UniversalInputManager.TypeOfInput.Mouse => mouse,
                UniversalInputManager.TypeOfInput.Joystick => joystick,
                _ => FinalInput
            };
            
            return FinalInput;
        }
     
        #endregion
    }
}