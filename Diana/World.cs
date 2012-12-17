using System;
using System.Collections.Generic;
using Diana.Util;

namespace Diana
{
    public class World
    {
        private readonly IDictionary<Type, IEntitySystem> _systemsByType;
        private readonly IBag<IEntitySystem> _allSystems;

        private readonly IBag<IManager> _managers;

        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;
        
        private readonly IBag<Entity> _added;
        private readonly IBag<Entity> _changed;
        private readonly IBag<Entity> _removed;

        public World()
        {
            _added = new Bag<Entity>();
            _changed = new Bag<Entity>();
            _removed = new Bag<Entity>();

            _systemsByType = new Dictionary<Type, IEntitySystem>();
            _allSystems = new Bag<IEntitySystem>();

            _managers = new Bag<IManager>();

            _entityManager = new EntityManager();
            AddManager(_entityManager);

            _componentManager = new ComponentManager();
            AddManager(_componentManager);

        }

        public EntityManager EntityManager { get { return _entityManager; } }
        public ComponentManager ComponentManager { get { return _componentManager; } }
        
        public T AddSystem<T>(T system, bool passive = false) where T : IEntitySystem
        {
            system.SetWorld(this);
            system.Passive = passive;

            _systemsByType[system.GetType()] = system;
            _allSystems.Add(system);

            system.Initialize();

            return system;
        }

        public void RemoveSystem(IEntitySystem system)
        {
            _systemsByType.Remove(system.GetType());
            _allSystems.Remove(system);
        }

        public void AddManager(IManager manager)
        {
            manager.SetWorld(this);
            _managers.Add(manager);
        }

        public void RemoveManager(IManager manager)
        {
           _managers.Remove(manager);
        }

        public int AddEntity(Action<IEntityBuilder> action)
        {
            var entityBuilder = new EntityBuilder(this);
            action(entityBuilder);

            var entity = entityBuilder.Build();
            _added.Add(entity);

            return entity.Id;
        }

        public void NotifyEntityChanged(Entity entity)
        {
            _changed.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _removed.Add(entity);
        }
        
        public void Process(double delta)
        {
            NotifyObservers(_added, (obs, e) => obs.Added(e));
            NotifyObservers(_changed, (obs, e) => obs.Changed(e));
            NotifyObservers(_removed, (obs, e) => obs.Removed(e));

            _componentManager.Clean();

            foreach (var system in _allSystems)
            {
                if (!system.Passive)
                    system.Process(delta);
            }
        }

        private void NotifyObservers(IBag<Entity> entities, Action<IEntityObserver, Entity> notifier)
        {
            if (entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    NotifySystems(notifier, entity);
                    NotifyManagers(notifier, entity);
                }
                entities.Clear();
            }
        }

        private void NotifySystems(Action<IEntityObserver, Entity> notifier, Entity entity)
        {
            foreach (var system in _allSystems)
            {
                notifier(system, entity);
            }
        }

        private void NotifyManagers(Action<IEntityObserver, Entity> notifier, Entity entity)
        {
            foreach (var manager in _managers)
            {
                notifier(manager, entity);
            }
        }
    }
}
