using System;
using System.Collections.Generic;

namespace Diana
{
    public class ComponentType
    {
        private static int _NextIndex;

        private Type _concreteType;
        private int _index;

        private ComponentType(Type type)
        {
            _concreteType = type;
            _index = _NextIndex++;
        }

        public int Index { get { return _index; } }

        public override string ToString() { return string.Format("ComponentType[{0}] ({1})", _concreteType.Name, _index); }

        private static readonly IDictionary<Type, ComponentType> RegisteredTypes = new Dictionary<Type, ComponentType>();

        public static ComponentType GetTypeFor<T>() where T : IComponent
        {
            return GetTypeFor(typeof (T));
        }

        public static ComponentType GetTypeFor(Type type)
        {
            if (RegisteredTypes.ContainsKey(type))
                return RegisteredTypes[type];

            var ctype = new ComponentType(type);
            RegisteredTypes[type] = ctype;

            return ctype;
        }

        public static int GetIndexFor(Type type)
        {
            return GetTypeFor(type).Index;
        }
    }
}