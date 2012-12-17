using System;
using System.Collections.Generic;
using Diana.Util;

namespace Diana
{
    public abstract class EntitySystem : IEntitySystem
    {
        private static int _NextIndex;

        protected Predicate<Entity> _entitySelector;

        protected World World;
        protected readonly int Index;
        protected readonly IBag<Entity> ActiveEntities;

        protected EntitySystem()
        {
            Index = _NextIndex++;
            ActiveEntities = new Bag<Entity>();
        }

        public void SetWorld(World world)
        {
            World = world;
        }

        public virtual void Initialize() { }
        public virtual void Shutdown() { }

        public bool Passive { get; set; }

        public void Process(double delta)
        {
            if (WillingToProcess())
            {
                Begin();
                ProcessEntities(ActiveEntities, delta);
                End();
            }
        }

        protected virtual bool WillingToProcess() { return true; }
        protected virtual void Begin() {}
        protected abstract void ProcessEntities(IEnumerable<Entity> activeEntities, double delta);
        protected virtual void End() {}

        public void Added(Entity entity)
        {
            Check(entity);
        }

        public void Changed(Entity entity)
        {
            Check(entity);
        }

        public void Removed(Entity entity)
        {
            if (entity.SystemBits.Get(this.Index))
            {
                Deactivate(entity);
            }
        }

        protected void Check(Entity entity) {
            var contains = entity.SystemBits.Get(this.Index);
            var interested = _entitySelector(entity);
		    
		    if (interested && !contains) {
			    Activate(entity);
		    } else if (!interested && contains) {
			    Deactivate(entity);
		    }
        }

        private void Activate(Entity entity)
        {
            ActiveEntities.Add(entity);
            entity.SystemBits.Set(Index, true);
            Activated(entity);
        }

        private void Deactivate(Entity entity)
        {
            ActiveEntities.Remove(entity);
            entity.SystemBits.Set(Index, false);
            Deactivated(entity);
        }

        protected virtual void Activated(Entity entity) {}
        protected virtual void Deactivated(Entity entity) {}
    }
}