using Diana.Util;

namespace Diana
{
    internal class EntityBuilder : IEntityBuilder
    {
        private readonly World _world;
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;
        private IBag<IComponent> _components = new Bag<IComponent>();

        public EntityBuilder(World world)
        {
            _world = world;
            _entityManager = world.EntityManager;
            _componentManager = world.ComponentManager;
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }

        public Entity Build()
        {
            var entity = _entityManager.CreateEntity();
            
            foreach (var component in _components)
            {
                _componentManager.AddComponent(entity, ComponentType.GetTypeFor(component.GetType()), component);
            }

            return entity;
        }
    }

    
}