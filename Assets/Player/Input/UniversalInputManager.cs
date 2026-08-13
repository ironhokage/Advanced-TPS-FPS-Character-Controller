using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Player.Input
{
    [DefaultExecutionOrder(-90)]
    public class UniversalInputManager : MonoBehaviour
    {
        public static UniversalInputManager Instance { get; private set; }

        [Header("Input Actions Reference")]
        [SerializeField] private PlayerInput playerInputActions;
        [SerializeField] private string playerActionMapName = "Player";
        [SerializeField] public bool debugInputs = true;

        public enum TypeOfInput
        {
            None,
            Mouse,
            Keyboard,
            Joystick,
            GamepadButtons
        };

        private InputActionMap _playerMap;

        public event Action OnJumpTriggered;
        public event Action OnJumpReleased;
        public event Action OnCrouchTriggered;
        public event Action OnCrouchReleased;
        public event Action OnRunStarted;
        public event Action OnRunStopped;
        public event Action OnDashTriggered;
        public event Action OnDashReleased;
        public event Action OnCamSwitchTriggered;
        public event Action OnCamSwitchReleased;

        public Vector2 MoveInput { get; private set; }
        public Vector2 CameraLookInput { get; set; }
        public Vector2 ZoomInput { get; set; }

        #region Initialization

        private void Awake()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInput();
        }

        private void InitializeInput()
        {
            if (playerInputActions == null)
            {
                Debug.LogError("UniversalInputManager: InputActionAsset is missing!");
                return;
            }

            _playerMap = playerInputActions.actions.FindActionMap(playerActionMapName);
            if (_playerMap == null)
            {
                Debug.LogError($"UniversalInputManager: Action Map '{playerActionMapName}' not found!");
                return;
            }

            RegisterPlayerInputActions();
        }

        #endregion

        #region Player Input Actions
        private void RegisterPlayerInputActions()
        {
            // Move
            _playerMap.FindAction("Move").performed += ctx => {
                MoveInput = ctx.ReadValue<Vector2>();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log($"[Input] Move: {MoveInput}");
            };
            _playerMap.FindAction("Move").canceled += ctx => {
                MoveInput = Vector2.zero;
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] Move: Canceled (Zeroed)");
            };

            // Camera
            _playerMap.FindAction("CameraLook").performed += ctx => {
                CameraLookInput = ctx.ReadValue<Vector2>();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log($"[Input] CameraLook: {CameraLookInput}");
            };

            _playerMap.FindAction("CameraLook").canceled += _ =>
            {
                CameraLookInput = Vector2.zero;
            };

            // Jump
            _playerMap.FindAction("Jump").performed += ctx => {
                OnJumpTriggered?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] Jump: Triggered");
            };
            _playerMap.FindAction("Jump").canceled += ctx => {
                OnJumpReleased?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] Jump: Released");
            };
            
            // CamSwitch
            _playerMap.FindAction("CamSwitch").performed += ctx => {
                OnCamSwitchTriggered?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] CamSwitch: Triggered");
            };
            _playerMap.FindAction("CamSwitch").canceled += ctx => {
                OnCamSwitchReleased?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] CamSwitch: Released");
            };

            // Run
            _playerMap.FindAction("Run").performed += ctx => {
                OnRunStarted?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] Run: Started");
            };
            _playerMap.FindAction("Run").canceled += ctx => {
                OnRunStopped?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] Run: Stopped");
            };

            // Zoom
            _playerMap.FindAction("Zoom").performed += ctx => {
                ZoomInput = ctx.ReadValue<Vector2>();
                DetectInputDevice(ctx);
                // Fixed the label here to say 'Zoom' and show the actual Vector2 value
                if (debugInputs) Debug.Log($"[Input] Zoom Value: {ZoomInput}"); 
            };

            _playerMap.FindAction("Zoom").canceled += ctx => {
                ZoomInput = Vector2.zero;
                DetectInputDevice(ctx);
            };

            // Dash
            _playerMap.FindAction("Dash").performed += ctx => {
                OnDashTriggered?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] Dash: Triggered");
            };
            _playerMap.FindAction("Dash").canceled += ctx => {
                OnDashReleased?.Invoke();
                DetectInputDevice(ctx);
                if (debugInputs) Debug.Log("[Input] Dash: Released");
            };
        }

        #endregion
        #region Input Device Detection
        public static TypeOfInput CurrentInputType = TypeOfInput.None;

        private void DetectInputDevice(InputAction.CallbackContext context)
        {
            var currentDevice = context.control;

            CurrentInputType = currentDevice switch
            {
                ButtonControl { device: Keyboard } => TypeOfInput.Keyboard,
                Vector2Control { device: Mouse } => TypeOfInput.Mouse,
                StickControl or Vector2Control { device: Joystick } => TypeOfInput.Joystick,
                ButtonControl { device: Gamepad } => TypeOfInput.GamepadButtons,
                _ => CurrentInputType
            };

            if (debugInputs)
                Debug.Log("Device: " + context.control.device.displayName + " | Control: " + context.control.displayName);
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change != InputDeviceChange.Added && change != InputDeviceChange.Removed) return;
            if (debugInputs)
                Debug.Log($"Device '{device}' was {change}");
        }
    
        #endregion
        private void OnEnable() {

            _playerMap?.Enable();
            InputSystem.onDeviceChange += OnDeviceChange;
        
        }
        private void OnDisable() 
        {
            _playerMap?.Disable();
            InputSystem.onDeviceChange -= OnDeviceChange;
       
        }
    }
}
