using System;
using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.NodeAccessors
{
    public class EnumerableNodeAccessor : NodeAccessorBase<IEnumerable>
    {
        private readonly Dictionary<string, object> _children;

        public EnumerableNodeAccessor()
        {
            _children = new Dictionary<string, object>();
        }

        protected override void OnWrap(IEnumerable data)
        {
            _children.Clear();

            var index = 0;

            foreach (var child in data)
            {
                var key = _keyMaker.SetCase(index + "]");

                _children[key] = child;

                index += 1;
            }
        }

        public override object GetChild(string key)
        {
            if (_children.ContainsKey(key))
            {
                return _children[key];
            }

            return null;
        }
        
        public override ICollection<string> GetChildren()
        {
            return _children.Keys;
        }

        public override bool Supports(Type type)
        {
            return Implements<IEnumerable>(type);
        }
        
        public override bool Supports(object @object)
        {
            return @object is IEnumerable;
        }

        public override string SeparatorFromParent => "[";
    }
}