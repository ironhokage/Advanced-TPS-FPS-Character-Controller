using KinematicCharacterController;
using UnityEngine;
using Player.StateMachines.Base;

namespace Player.Interfaces.Movement
{
    // For scripts that handle physics, gravity, and slopes (e.g., Falling, Sliding)
    public interface IMovementContext
    {
        Vector3 GroundNormal { get; }
        Vector3 DownhillDir { get; }
        bool IsStableOnGround { get; }
        bool FoundAnyGround { get; }
    }

    // For scripts that calculate directional movement and input vectors (e.g., Walking, Dashing, Camera)
    public interface IOrientationDataProvider
    {
        Vector3 CharacterUp { get; }
        Vector3 ForwardBody { get; }
        Vector3 RightBody { get; }
        Vector3 UpBody { get; }
    }

    // For scripts tracking time and core state logic (e.g., Animations, Combo steps)
    public interface IDataProvider
    {
        float ClampedDeltaTime { get; }
        KinematicCharacterMotor Motor { get; }
        StateMachine PlayerState { get; }
    }
}