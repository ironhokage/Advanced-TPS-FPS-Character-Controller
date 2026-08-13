using Player.Managers.Helpers;
using UnityEngine;

namespace Player.Interfaces.Movement
{
    public interface IGroundMovementAnimationHooks
    {
         float SpeedRatio { get; }         // Normalized speed (0-1)
         float ForwardDot { get; }          // How aligned input is with kccMovement forward
         float RightDot { get; }
         Vector3 ForwardBody { get; }
         Vector3 RightBody { get; }
         float LocalForwardVelocity { get; }
         float LocalRightVelocity { get; }
    }

    public interface IGroundMovementInputHooks
    {
        float InputMagnitude { get; }     // Raw input magnitude
        Vector3 InputDirection { get; }
        Vector3 SmoothedInputDirection { get; }
    }

    public interface IGroundMovementDirectionHooks
    {
        float Alignment { get; }          // How aligned current movement is with previous
        float TurnRate { get; }             // Angular speed / rotation delta
        Vector3 MoveDirection { get; } 
        PlayerGroundMovementHelper.SlopeData SlopeData { get; }
    }
}
