using Player.Managers;
using Player.Managers.Movement;
using Player.Scripts.Managers;
using UnityEngine;

namespace Player.Scripts.StateMachines.Base
{
    public abstract class BaseState : IState
    {
        protected readonly KCCMovementCharacterController KccMovement;
        protected readonly Animator animator;
        
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
        
        protected BaseState(KCCMovementCharacterController kccMovement, Animator animator)
        {
            this.KccMovement = kccMovement;
            this.animator = animator;
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnFixedUpdate()
        {
            
        }

        public virtual void OnExit()
        {
           
        }
    }
}