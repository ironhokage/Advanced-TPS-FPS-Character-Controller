namespace Player.Spring.Runtime
{
    public interface INudgeable<in T>
    {
        void Nudge(T amount);
    }
}