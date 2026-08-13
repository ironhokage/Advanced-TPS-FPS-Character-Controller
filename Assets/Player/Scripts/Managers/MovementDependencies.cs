using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.Input;
using Player.Locomotion.Interface;
using Player.Locomotion.Locomotion;
using Player.Managers.Interfaces;
using Player.Managers.Movement;
using Player.PlayerSettings.Movement;
using UnityEngine;

namespace Player.Managers
{
   public class MovementDependencies : IMovementDependency, ICharacterMovementHandler
{
    private readonly PlayerRunManager _runManager = new();
    private readonly PlayerJumpManager _jumpManager = new() { IsJumpActive = false };
    private readonly PlayerGroundMovementManager _groundMovementManager = new();
    private readonly PlayerInAirMovementManager _playerInAirMovementManager = new();

    public bool IsRunning => _runManager.IsRunning;
    public bool IsJumpActive => _jumpManager.IsJumpActive;

    public KinematicCharacterMotor Motor { get; set; }
    public UniversalInputManager InputManager { get; set; }

    public bool IsActive { get; set; }
    
    private Vector2 _rawInput;
    private Vector3 _moveInputVector;

    public async UniTask InitializeMovementDependencies(CancellationTokenSource ctx)
    {
        try
        {
            await _groundMovementManager.Initialize().AttachExternalCancellation(ctx.Token);
            await _playerInAirMovementManager.Initialize().AttachExternalCancellation(ctx.Token);
            await _jumpManager.InitializeJumpSett().AttachExternalCancellation(ctx.Token);
            
            _jumpManager.Motor = Motor;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }

        IsActive = true;
        _jumpManager.CanJump = IsActive;
    }
    
    public void UpdateRawInput() => _rawInput = Vector2.ClampMagnitude(InputManager.MoveInput, 1f);
    
    public void ComputeMovementInput(
        Vector3 charForward, Vector3 charRight, Vector3 charUp,
        Vector3 camForward, Vector3 camRight, Vector3 camUp,
        bool isInFPS)
    {
        // --- FPS: character‑relative (unchanged) ---
        var charFwd = Vector3.ProjectOnPlane(charForward, charUp).normalized;
        var charRgt = Vector3.ProjectOnPlane(charRight, charUp).normalized;
        if (charFwd.sqrMagnitude < 0.001f)
            charFwd = Vector3.ProjectOnPlane(charUp, charUp).normalized;
        Vector3 charMove = charFwd * _rawInput.y + charRgt * _rawInput.x;

        // --- TPS: camera‑relative ---
        // Use world up for projection so vertical camera tilt doesn't skew movement
        Vector3 worldUp = Vector3.up;
        var camFwdFlat = Vector3.ProjectOnPlane(camForward, worldUp).normalized;
        var camRgtFlat = Vector3.ProjectOnPlane(camRight, worldUp).normalized;
        if (camFwdFlat.sqrMagnitude < 0.001f)
            camFwdFlat = Vector3.ProjectOnPlane(worldUp, worldUp).normalized;
        Vector3 camMove = camFwdFlat * _rawInput.y + camRgtFlat * _rawInput.x;

        _moveInputVector = isInFPS ? charMove : camMove;
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, bool isStableOnGround, float deltaTime, Vector3 up)
    {
        if (!IsActive) return;
        if (isStableOnGround)
        {
            _groundMovementManager.HandleGroundMovement(ref currentVelocity,  _moveInputVector);
        }
        else
        {
            currentVelocity += up * (-30.0f * deltaTime);
            currentVelocity *= 1f / (1f + 0.2f * deltaTime);
            _playerInAirMovementManager.HandleInAirMovement(ref currentVelocity,  _moveInputVector);
        }

        _jumpManager.ApplyJumpForce(ref currentVelocity);
    }
    
    public void UpdateCharacterRotation(float deltaTime, float deltaAngle, bool isInFps)
    {
        if (isInFps)
        {
            Motor.SetRotation(Quaternion.AngleAxis(deltaAngle, Motor.CharacterUp), bypassInterpolation: true);
            return;
        }

        if (_moveInputVector.sqrMagnitude > 0.001f)
        {
            var targetRotation = Quaternion.LookRotation(_moveInputVector, Vector3.up);
            Motor.SetRotation(targetRotation, bypassInterpolation: true);
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        if (!IsActive) return;
        _jumpManager.UpdateJumpBuffer(deltaTime);
    }

    public void EnableDependency()
    {
        InputManager.OnJumpTriggered += _jumpManager.HandleJumpPressed;
        InputManager.OnJumpReleased += _jumpManager.HandleJumpReleased;
        
        InputManager.OnRunStarted += OnRunStarted;
        InputManager.OnRunStopped += _runManager.HandleRunReleased;
    }

    public void DisableDependency()
    {
        InputManager.OnJumpTriggered -= _jumpManager.HandleJumpPressed;
        InputManager.OnJumpReleased -= _jumpManager.HandleJumpReleased;
        
        InputManager.OnRunStarted -= OnRunStarted;
        InputManager.OnRunStopped -= _runManager.HandleRunReleased;
    }

    private void OnRunStarted()
    {
        if (!IsActive) return;
        _runManager.HandleRunPressed(_rawInput.y);
    }

    public MovementSettings MovementSettings
    {
        get => _groundMovementManager.Settings;
        set => _groundMovementManager.Settings = value;
    }
}
}
