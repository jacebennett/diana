namespace Diana
{
    public interface IEntityObserver
    {
        void Added(Entity entity);
        void Changed(Entity entity);
        void Removed(Entity entity);
    }
}