using Player.PlayerSettings.Movement;
using UnityEngine;

namespace Player.Locomotion.Interface
{
    public interface ICharacterMovementHandler
    {
        // Movement
        void UpdateVelocity(ref Vector3 currentVelocity, bool isStableOnGround, float deltaTime, Vector3 up);

        // Lifecycle
        void BeforeCharacterUpdate(float deltaTime);

        void ComputeMovementInput(
            Vector3 charForward, Vector3 charRight, Vector3 charUp,
            Vector3 camForward, Vector3 camRight, Vector3 camUp,
            bool isInFPS);

        // Input
        void EnableDependency();
        void DisableDependency();

        void UpdateCharacterRotation(float deltaTime, float deltaAngle, bool isInFps);
        void UpdateRawInput();

        // Data for state machine predicates & settings
        bool IsRunning { get; }
        bool IsJumpActive { get; }
        MovementSettings MovementSettings { get; }
    }
}