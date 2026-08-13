namespace Player.Interfaces.Movement.Context
{
    public static class GroundMovementContext
    {
        public static IGroundMovementDirectionHooks DirectionHooks { get; set; }
        public static IGroundMovementAnimationHooks MovementHooks { get; set; }
        public static IGroundMovementInputHooks InputHooks { get; set; }
    }
}
