using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.NodeAccessors
{
    public class ObjectPropertyNodeAccessor : INodeAccessor
    {
        private readonly Dictionary<string, PropertyInfo> _propertyInfos;

        private Type _type = typeof(object);

        private object _data = new object();

        private KeyMaker _keyMaker = new KeyMaker();

        public ObjectPropertyNodeAccessor()
        {
            _propertyInfos = new Dictionary<string, PropertyInfo>();
        }

        public object GetChild(string key)
        {
            key = _keyMaker.SetCase(key);

            if (_propertyInfos.ContainsKey(key))
            {
                try
                {
                    var property = _propertyInfos[key];

                    var value = property.GetValue(_data);

                    return value;
                }
                catch (Exception e)
                {
                }
            }

            return null;
        }


        public ICollection<string> GetChildren()
        {
            return _propertyInfos.Keys;
        }

        public bool Supports(Type type)
        {
            return true;
        }

        public bool Supports(object @object)
        {
            return true;
        }

        public void Wrap(object data, Type type, KeyMaker keyMaker)
        {
            _data = data;

            _type = type;

            _keyMaker = keyMaker;

            _propertyInfos.Clear();

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                //POCO
                if (property.CanRead && property.CanWrite)
                {
                    var key = keyMaker.SetCase(property.Name);

                    _propertyInfos.Add(key, property);
                }
            }
        }

        public string SeparatorFromParent => ".";
    }
}