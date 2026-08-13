using Player.Managers;
using Player.Managers.Movement;
using Player.Scripts.Managers;
using Player.Scripts.StateMachines.Base;
using UnityEngine;

namespace Player.StateMachines.SpecialStates
{
    public class DashState : BaseState
    {
        public DashState(KCCMovementCharacterController kccMovement, Animator animator) : base(kccMovement, animator)
        {
            
        }
    }
}
