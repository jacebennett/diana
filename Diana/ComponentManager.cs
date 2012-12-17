using Diana.Util;

namespace Diana
{
    public class ComponentManager : Manager
    {
        private readonly Bag<Bag<IComponent>> _componentsByType; 
        private readonly Bag<Entity> _deleted;

        public ComponentManager()
        {
            _componentsByType = new Bag<Bag<IComponent>>();
            _deleted = new Bag<Entity>();
        }

        public Bag<IComponent> GetComponentsOfType(ComponentType componentType)
        {
            Bag<IComponent> components = _componentsByType[componentType.Index];
            if (components == null)
            {
                components = new Bag<IComponent>();
                _componentsByType[componentType.Index] = components;
            }
            return components;
        }

        public void AddComponent(Entity entity, ComponentType componentType, IComponent component)
        {
            _componentsByType.EnsureCapacity(componentType.Index+1);
            var components = GetComponentsOfType(componentType);
            components[entity.Id] = component;

            entity.ComponentBits.Set(componentType.Index, true);
        }

        public void RemoveComponent(Entity entity, ComponentType type)
        {
            if (entity.ComponentBits[type.Index])
            {
                _componentsByType[type.Index][entity.Id] = null;
                entity.ComponentBits[type.Index] = false;
            }
        }

        private void RemoveComponentsOfEntity(Entity entity)
        {
            var componentBits = entity.ComponentBits;
            for (var i = 0; i < componentBits.Count; i++)
            {
                if (componentBits[i])
                {
                    _componentsByType[i][entity.Id] = null;
                }
            }
            componentBits.SetAll(false);
        }

        public override void Removed(Entity entity)
        {
            _deleted.Add(entity);
            base.Removed(entity);
        }

        public void Clean()
        {
            if (_deleted.Count > 0)
            {
                foreach (var entity in _deleted)
                {
                    RemoveComponentsOfEntity(entity);
                }
                _deleted.Clear();
            }
        }

    }
}