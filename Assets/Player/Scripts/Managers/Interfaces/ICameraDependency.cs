using System.Threading;
using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.Input;
using UnityEngine;

namespace Player.Managers.Interfaces
{
    public interface ICameraDependency
    {
        UniTask InitializeCameraDependencies(CancellationTokenSource ctx);
        
        void UpdateDependency(UniversalInputManager inputManager, bool isInFPS);
        void LateUpdateDependency(bool isInFPS);
        
        public bool IsCameraActive { get; set; }
        float TargetYaw { get; set; }

        void CameraSwitcher(bool isInFPS);

        KinematicCharacterMotor CharacterMotor { get; set; }

    }
}
