using System.Collections.Generic;
using Diana.Util;

namespace Diana
{
    public class EntityManager : Manager
    {
        private readonly IBag<Entity> _entities = new Bag<Entity>(); 
        private readonly EntityIdPool _idPool = new EntityIdPool();
        
        public Entity CreateEntity()
        {
            return new Entity(World, _idPool.CheckOut());
        }

        public override void Added(Entity entity)
        {
            _entities[entity.Id] = entity;
            base.Added(entity);
        }

        public override void Removed(Entity entity)
        {
            _entities[entity.Id] = null;

            _idPool.CheckIn(entity.Id);
            base.Removed(entity);
        }

        public bool IsActive(int entityId)
        {
            return _entities[entityId] != null;
        }
    }

    internal class EntityIdPool
    {
        private readonly Queue<int> _reusableIds;
        private int _nextAvailableId;

        public EntityIdPool()
        {
            _reusableIds = new Queue<int>();
        }

        public int CheckOut()
        {
            if (_reusableIds.Count > 0)
            {
                return _reusableIds.Dequeue();
            }
            return _nextAvailableId++;
        }

        public void CheckIn(int id)
        {
            _reusableIds.Enqueue(id);
        }
    }
}