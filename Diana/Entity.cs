using System;
using System.Collections;

namespace Diana
{
    public sealed class Entity
    {
        private readonly Guid _guid;
        private readonly World _world;
        private readonly int _id;
        private readonly ComponentManager _componentManager;
        private readonly BitArray _systemBits;
        private readonly BitArray _componentBits;

        public Entity(World world, int id)
        {
            _guid = Guid.NewGuid();
            _world = world;
            _id = id;
            _componentManager = world.ComponentManager;
            _systemBits = new BitArray(32);
            _componentBits = new BitArray(32);
        }

        public Guid Guid { get { return _guid; } }
        public int Id { get { return _id; } }
        public BitArray SystemBits { get { return _systemBits; } }
        public BitArray ComponentBits { get { return _componentBits; } }

        public void AddComponent(IComponent component)
        {
            AddComponent(component, ComponentType.GetTypeFor(component.GetType()));
        }

        public void AddComponent(IComponent component, ComponentType type)
        {
            _componentManager.AddComponent(this, type, component);
            _world.NotifyEntityChanged(this);
        }

        public void RemoveComponent(IComponent component)
        {
            RemoveComponent(ComponentType.GetTypeFor(component.GetType()));
        }

        public void RemoveComponent(ComponentType type)
        {
            _componentManager.RemoveComponent(this, type);
            _world.NotifyEntityChanged(this);
        }
    }
}