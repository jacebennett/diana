namespace Diana
{
    public interface IEntitySystem : IEntityObserver
    {
        void SetWorld(World world);
        void Initialize();

        bool Passive { get; set; }
        void Process(double delta);

        void Shutdown();
    }
}