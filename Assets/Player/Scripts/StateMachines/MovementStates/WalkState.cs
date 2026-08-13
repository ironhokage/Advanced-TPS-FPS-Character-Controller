using Player.Managers;
using Player.Managers.Movement;
using Player.PlayerSettings.Movement;
using Player.Scripts.Locomotion.Interface;
using Player.Scripts.StateMachines.Base;
using UnityEngine;

namespace Player.StateMachines.MovementStates
{
    public class WalkState : BaseState
    {
        private readonly WalkSettings _walkSettings;
        private readonly ICharacterMovementHandler _handler;

        // Constructor now takes the handler instead of raw MovementSettings
        public WalkState(KCCMovementCharacterController controller, Animator animator,
            WalkSettings walkSettings, ICharacterMovementHandler handler)
            : base(controller, animator)
        {
            _walkSettings = walkSettings;
            _handler = handler;
        }
    
        public override void OnEnter()
        {
            
            var moveSettings = _handler.MovementSettings;
            if (moveSettings == null) return;
            
            moveSettings.CurrentMovementSpeed = _walkSettings.WalkSpeed;
            moveSettings.CurrentAccelerationSpeed = _walkSettings.WalkAccel;
            moveSettings.CurrentDecelerationSpeed = _walkSettings.WalkDecel;
        }
    }
}