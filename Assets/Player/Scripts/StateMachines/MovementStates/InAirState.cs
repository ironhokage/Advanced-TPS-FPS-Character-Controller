using Player.Managers;
using Player.Managers.Movement;
using Player.Scripts.Managers;
using Player.Scripts.StateMachines.Base;
using UnityEngine;

namespace Player.StateMachines.MovementStates
{
    public class InAirState : BaseState
    {
        public InAirState(KCCMovementCharacterController kccMovement, Animator animator) : base(kccMovement, animator)
        {
        
        }
    }
}
