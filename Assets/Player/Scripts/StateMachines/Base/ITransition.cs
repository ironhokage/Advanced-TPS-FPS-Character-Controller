namespace Player.Scripts.StateMachines.Base
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}