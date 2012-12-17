namespace Diana
{
    public abstract class Manager : IManager
    {
        protected World World;

        public void SetWorld(World world)
        {
            World = world;
        }

        public virtual void Added(Entity entity) {}
        public virtual void Changed(Entity entity) {}
        public virtual void Removed(Entity entity) {}
    }
}