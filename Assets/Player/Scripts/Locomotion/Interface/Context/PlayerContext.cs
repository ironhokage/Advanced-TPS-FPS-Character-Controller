namespace Player.Interfaces.Movement.Context
{
    public static class PlayerContext
    {
        // Globally accessible interface references
        public static IMovementContext Movement { get; set; }
        public static IDataProvider State { get; set; }
        public static IOrientationDataProvider Orientation { get; set; }
    }
}