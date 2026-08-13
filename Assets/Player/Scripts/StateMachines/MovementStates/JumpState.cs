using Player.Managers;
using Player.Managers.Movement;
using Player.Scripts.Managers;
using Player.Scripts.StateMachines.Base;
using UnityEngine;

namespace Player.Scripts.StateMachines.MovementStates
{
    public class JumpState : BaseState
    {
        public JumpState(KCCMovementCharacterController kccMovement, Animator animator) : base(kccMovement, animator) { }

        public override void OnEnter()
        {
            Debug.Log("Jump State: OnEnter");
        }

        public override void OnExit()
        {
            Debug.Log("Jump State: OnExit");
        }


    }
}
