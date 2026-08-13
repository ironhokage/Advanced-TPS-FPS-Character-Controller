using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.Managers.Interfaces
{
    public interface IMovementDependency
    {
         UniTask InitializeMovementDependencies(CancellationTokenSource ctx);
         
         KinematicCharacterMotor Motor { get; set; }
         UniversalInputManager InputManager { get; set; }

         void EnableDependency();
         void DisableDependency();
         
         bool IsActive { get; set; }
        
    }
}
