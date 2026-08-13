using Cysharp.Threading.Tasks;
using Player.PlayerSettings.Movement;

namespace Player.Managers.Interfaces
{
    public interface IGroundMovement
    {
        public UniTask<MovementSettings> Initialize();
        MovementSettings Settings { get; set; }
    }
}
