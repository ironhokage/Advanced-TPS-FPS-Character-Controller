using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.Input;
using Player.Interfaces.Movement;
using Player.Interfaces.Movement.Context;
using Player.Managers;
using Player.Managers.Movement;
using Player.PlayerSettings.Movement;
using Player.StateMachines.Base;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Scripts.Managers
{
    [DefaultExecutionOrder(-60)] 
    public class PlayerBrainController : MonoBehaviour, IMovementContext, IOrientationDataProvider, IDataProvider
    {
        [Header("Cinemachine Virtual Cameras")]
        [Tooltip("The cameras used by the kccMovement")]
        [SerializeField] private List<CinemachineCamera> cameras = new();
        
        [Space(2)]
        [Header("Global input handler")]
        [Tooltip("Used to provide input and key mappings")]
        [SerializeField] private UniversalInputManager inputManager;

        [Space(2)]
        [Header("KCC Motor")]
        [Tooltip("KCC Motor used for movement")]
        [SerializeField] private KinematicCharacterMotor kccMotor;
        
        [Space(2)]
        [Header("Movement Settings")]
        [SerializeField] private WalkSettings walkSettings;
        [SerializeField] private RunSettings runSettings;
        
        private readonly KCCMovementCharacterController kccMovementCharacterController = new();
        private readonly MovementDependencies _movementDependency = new();
        private readonly CameraDependencies _cameraDependency = new();
        private CancellationTokenSource _cts;
        private bool _isInFPS = true;

        private void Awake()
        {
            ValidateDependencies();
            SetupInterfaces();
            
            kccMovementCharacterController.AwakeKCCMotor();
            _cts = new CancellationTokenSource();
            
            _cameraDependency.GetCameraReferences(cameras);
            InitializeAsync().Forget(); 
        }

        private async UniTask InitializeAsync()
        {
            try
            {
                await _cameraDependency.InitializeCameraDependencies(_cts);
                await _movementDependency.InitializeMovementDependencies(_cts);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Initialization cancelled"); 
            }
            catch (Exception e)
            {
                Debug.LogError($"Initialization failed: {e}");
            }
        }
        
        private void ValidateDependencies()
        {
            if (kccMovementCharacterController != null)
            {
                kccMotor.CharacterController = kccMovementCharacterController;
                
                kccMovementCharacterController.WalkSettings = walkSettings;
                kccMovementCharacterController.RunSettings = runSettings;
                kccMovementCharacterController.MovementHandler = _movementDependency;
                kccMovementCharacterController.Motor = kccMotor;
                
                _movementDependency.Motor = Motor;
                _movementDependency.InputManager = inputManager;
                
                _cameraDependency.CharacterMotor = kccMotor;
            }
            else
            {
                Debug.LogError("KccMovementCharacterController is null");
            }
        }
        
        private void SetupInterfaces()
        {
            PlayerContext.Movement = this;
            PlayerContext.State = this;
            PlayerContext.Orientation = this;
        }

        private void FixedUpdate()
        {
            kccMovementCharacterController.FixedUpdateKCCMotor();
        }

        private void Update()
        {
            _cameraDependency.UpdateDependency(inputManager, _isInFPS);
            _movementDependency.UpdateRawInput();
            
            var cam = _cameraDependency._tpsCamera.transform;

            _movementDependency.ComputeMovementInput(
                Motor.CharacterForward, Motor.CharacterRight, Motor.CharacterUp,
                cam.forward, Vector3.up, 
                _isInFPS);

            _movementDependency.UpdateCharacterRotation(
                ClampedDeltaTime, _cameraDependency.TargetYaw, _isInFPS);

            kccMovementCharacterController.UpdateKCCMotor();
        }

        private void LateUpdate()
        {
            _cameraDependency.LateUpdateDependency(_isInFPS);
        }
        
        private void OnCamSwitchTriggered()
        {
            _cameraDependency.OnCamSwitchTriggered(ref _isInFPS);
        }
        
        private void OnEnable()
        {
            inputManager.OnCamSwitchTriggered += OnCamSwitchTriggered;
            _cameraDependency.CameraSwitcher(_isInFPS);
            _movementDependency.EnableDependency();
        }

        private void OnDisable()
        {
            inputManager.OnCamSwitchTriggered -= OnCamSwitchTriggered;
            _movementDependency.DisableDependency();
            
            _cts?.Cancel();
            _cts?.Dispose();
        }

        #region Character Ground 
        public Vector3 GroundNormal => kccMotor.GroundingStatus.GroundNormal;
        public bool IsStableOnGround => kccMotor.GroundingStatus.IsStableOnGround;
        public bool FoundAnyGround => kccMotor.GroundingStatus.FoundAnyGround;
        public Vector3 DownhillDir =>
            Vector3.ProjectOnPlane(-kccMotor.CharacterUp, kccMotor.GroundingStatus.GroundNormal).normalized;
        #endregion
        
        #region Character Axes
        public Vector3 CharacterUp => Motor.CharacterUp;
        public Vector3 ForwardBody => kccMotor.CharacterForward;
        public Vector3 RightBody => kccMotor.CharacterRight;
        public Vector3 UpBody => kccMotor.CharacterUp;
        #endregion

        #region Misc
        public KinematicCharacterMotor Motor => kccMotor;
        public StateMachine PlayerState => kccMovementCharacterController.PlayerStateMachine;
        public float ClampedDeltaTime => Mathf.Min(Time.deltaTime, 0.033f);
        #endregion
    }
}