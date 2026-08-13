using Cysharp.Threading.Tasks;
using KinematicCharacterController;
using Player.PlayerSettings.Jump;

namespace Player.Managers.Interfaces
{
    public interface IJump
    {
        UniTask<JumpSettings> InitializeJumpSett();
        JumpSettings Settings { get; set; }
        KinematicCharacterMotor Motor { get; set; }
    }
}
