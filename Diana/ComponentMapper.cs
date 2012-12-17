using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diana.Util;

namespace Diana
{
    public class ComponentMapper<TComponent> where TComponent : class, IComponent
    {
        private Bag<IComponent> _componentsOfThisType;
        private ComponentType _componentType;

        public ComponentMapper(World world)
        {
            _componentType = ComponentType.GetTypeFor<TComponent>();
            _componentsOfThisType = world.ComponentManager.GetComponentsOfType(_componentType);
        }

        public ComponentType ComponentType { get { return _componentType; } }

        public TComponent Map(Entity entity)
        {
            return (TComponent)_componentsOfThisType[entity.Id];
        }

        public TComponent SafeMap(Entity entity)
        {
            if(_componentsOfThisType.Count > entity.Id)
                return (TComponent)_componentsOfThisType[entity.Id];
            return null;
        }

        public bool Has(Entity entity)
        {
            return SafeMap(entity) != null;
        }
    }
}
