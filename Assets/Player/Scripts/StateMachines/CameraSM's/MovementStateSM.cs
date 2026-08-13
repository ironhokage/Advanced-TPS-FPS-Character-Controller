using Player.Interfaces.Movement.Context;

namespace Player.StateMachines.CameraSM_s
{
    public class MovementStateSM
    {
        #region Enums

        public enum MoveState
        {
            Idle,
            Start,
            Stop,
            Moving
        }
        
        #endregion
        
        public MoveState CharMovementState;

        #region Interface Implementation
        
        public float SpeedRatio => GroundMovementContext.MovementHooks != null ? GroundMovementContext.MovementHooks.SpeedRatio : 0f;
        public float InputMagnitude => GroundMovementContext.InputHooks != null ? GroundMovementContext.InputHooks.InputMagnitude : 0f;

        #endregion
        
        public MoveState UpdateMoveState()
        {
            var charSpeed = SpeedRatio;
            var charDirection = InputMagnitude;
            
            CharMovementState = charSpeed switch
            {
                < 0.001f when charDirection < 0.001f => MoveState.Idle, 
                < 0.001f when charDirection > 0.001f => MoveState.Start,
                < 1.0f when charDirection < 0.001f => MoveState.Stop,
                _ => MoveState.Moving
            };
            
            return CharMovementState;
        }
    }
}