using Player.Managers;
using Player.Managers.Movement;
using Player.PlayerSettings.Movement;
using Player.Scripts.Locomotion.Interface;
using Player.Scripts.StateMachines.Base;
using UnityEngine;

namespace Player.StateMachines.MovementStates
{
    public class RunState : BaseState
    {
        private readonly ICharacterMovementHandler _handler;
        private readonly RunSettings _runSettings;
        
        public RunState(KCCMovementCharacterController kccMovement, Animator animator, RunSettings runSettings, ICharacterMovementHandler handler) 
            : base(kccMovement, animator)
        {
            _handler = handler;
            _runSettings = runSettings;
        }

        public override void OnEnter()
        {
            
            var moveSettings = _handler.MovementSettings;
            if (moveSettings == null) return;
            
            moveSettings.CurrentMovementSpeed = _runSettings.RunSpeed;
            moveSettings.CurrentAccelerationSpeed = _runSettings.RunAccel;
            moveSettings.CurrentDecelerationSpeed = _runSettings.RunDeccel;
        }
    }
}