using KinematicCharacterController;
using Player.Locomotion.Interface;
using Player.Managers.Interfaces;
using Player.PlayerSettings.Movement;
using Player.Scripts.StateMachines.Base;
using Player.Scripts.StateMachines.MovementStates;
using Player.StateMachines.Base;
using Player.StateMachines.MovementStates;
using UnityEngine;

namespace Player.Managers.Movement
{
    public class KCCMovementCharacterController : 
        IKCCMovement,
        ICharacterController
    {
        #region Initialization Variables
        public WalkSettings WalkSettings { get; set; }
        public RunSettings RunSettings { get; set; }
        #endregion
        
        #region Misc
        public KinematicCharacterMotor Motor { get; set; }
        public StateMachine PlayerStateMachine { get; set; }
        public ICharacterMovementHandler MovementHandler { get; set; }
        #endregion
        
        private bool _isInitialized;
        
        public void AwakeKCCMotor()
        {
            SetupStateMachine();
            _isInitialized = true;
        }

        public void FixedUpdateKCCMotor()
        {
            if (_isInitialized && MovementHandler != null && PlayerStateMachine != null)
                PlayerStateMachine.FixedUpdate();
        }
        
        public void UpdateKCCMotor()
        {
            if (!_isInitialized || MovementHandler == null || PlayerStateMachine == null) return;
            try
            {
                PlayerStateMachine.Update(); 
            }
            catch (System.Exception e) { Debug.LogError($"StateMachine error: {e}"); }
        }

        #region Setup Procedures
        private void SetupStateMachine()
        {
            PlayerStateMachine = new StateMachine();

            // 1. Declare states
            var idleState = new IdleState(this, animator: null);
            var walkState = new WalkState(this, animator: null, WalkSettings, MovementHandler);
            var runState = new RunState(this, animator: null, RunSettings, MovementHandler);
            var jumpState = new JumpState(this, animator: null);
            
            // 2. Declare transitions
            At(idleState, walkState, new FuncPredicate(() => Motor.Velocity.magnitude > 0.01f));
            At(walkState, idleState, new FuncPredicate(() => Motor.Velocity.magnitude < 0.05f));
            At(idleState, jumpState, new FuncPredicate(() => MovementHandler.IsJumpActive && Motor.GroundingStatus.IsStableOnGround));
            At(jumpState, idleState, new FuncPredicate(() => !MovementHandler.IsJumpActive && Motor.GroundingStatus.IsStableOnGround)); 
            At(walkState, runState, new FuncPredicate(() => MovementHandler.IsRunning));
            At(runState, walkState, new FuncPredicate(() => !MovementHandler.IsRunning));
            
            // 3. Set the initial state
            PlayerStateMachine.SetState(idleState);
        }
        #endregion

        #region State Machine Helpers
        private void At(IState from, IState to, IPredicate condition) => PlayerStateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => PlayerStateMachine.AddAnyTransition(to, condition);
        #endregion

        #region ICharacterController Implementation
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) { }
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) => MovementHandler?.UpdateVelocity(ref currentVelocity, 
            Motor.GroundingStatus.IsStableOnGround, deltaTime, Motor.CharacterUp);
        public void BeforeCharacterUpdate(float deltaTime) => MovementHandler?.BeforeCharacterUpdate(deltaTime); 
        public void AfterCharacterUpdate(float deltaTime) { }
        public void PostGroundingUpdate(float deltaTime) { }
        public bool IsColliderValidForCollisions(Collider coll) => true;
        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
        public void OnDiscreteCollisionDetected(Collider hitCollider) { }
        #endregion
    }
} 