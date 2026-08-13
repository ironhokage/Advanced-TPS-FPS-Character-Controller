using Cysharp.Threading.Tasks;
using Player.Scripts.PlayerSettings.Movement;

namespace Player.Managers.Interfaces
{
    public interface IAirMovement
    {
        public UniTask<InAirMovementSettings> Initialize();
        InAirMovementSettings Settings { get; set; }
    }
}
