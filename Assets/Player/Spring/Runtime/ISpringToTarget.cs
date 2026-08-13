namespace Player.Spring.Runtime
{
    public interface ISpringTo<in T>
    {
        void SpringTo(T target);
    }
}
