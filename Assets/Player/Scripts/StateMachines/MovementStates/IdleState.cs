using Player.Managers;
using Player.Managers.Movement;
using Player.Scripts.StateMachines.Base;
using UnityEngine;

namespace Player.StateMachines.MovementStates
{
    public class IdleState : BaseState
    {
        public IdleState(KCCMovementCharacterController kccMovement, Animator animator) : base(kccMovement, animator) { }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnter()
        {
           // Debug.Log("Entering IdleState");
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnExit()
        {
            //Debug.Log("Exiting IdleState");
        }
    }
}
