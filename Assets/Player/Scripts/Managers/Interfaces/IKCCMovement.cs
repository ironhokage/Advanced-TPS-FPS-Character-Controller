using KinematicCharacterController;
using Player.Input;
using Player.PlayerSettings.Movement;
using Player.StateMachines.Base;
using UnityEngine;

namespace Player.Managers.Interfaces
{
    public interface IKCCMovement
    {
        #region Player Settings
        WalkSettings WalkSettings { get; set; }
        RunSettings RunSettings { get; set; }
        #endregion
        
        StateMachine PlayerStateMachine { get; set; }
        KinematicCharacterMotor Motor { get; set; }
    }
}
