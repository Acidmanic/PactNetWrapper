using System.Reflection;

namespace Pact.Provider.Wrapper.PactPort
{
    public class PropertyWrapper
    {
        private readonly object _owner;
        private readonly PropertyInfo _accessor;

        public PropertyWrapper(PropertyInfo accessor, object owner)
        {
            _accessor = accessor;
            _owner = owner;
        }

        public object Get()
        {
            if (_accessor != null && _accessor.CanRead)
            {
                return _accessor.GetValue(_owner);
            }

            return null;
        }

        public bool Set(object value)
        {
            if (_accessor != null && _accessor.CanWrite)
            {
                _accessor.SetValue(_owner, value);

                return true;
            }

            return false;
        }
    }
}