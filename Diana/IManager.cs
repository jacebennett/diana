namespace Diana
{
    public interface IManager : IEntityObserver
    {
        void SetWorld(World world);
    }
}