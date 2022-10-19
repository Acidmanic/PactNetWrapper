using System;
using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.NodeAccessors
{
    public class HashTableNodeAccessor:NodeAccessorBase<Hashtable>
    {
        public override object GetChild(string key)
        {
            if (_data is Hashtable hashMe)
            {
                if (hashMe.ContainsKey(key))
                {
                    return hashMe[key];
                }
            }
            return null;
        }

        public override ICollection<string> GetChildren()
        {
            var children = new List<string>();
            
            if (_data is Hashtable hashMe)
            {
                foreach (var key in hashMe.Keys)
                {
                        children.Add(key.ToString());
                }
            }
            return children;
        }

        public override bool Supports(Type type)
        {
            return IsDerivedFrom<Hashtable>(type);
        }

        public override bool Supports(object @object)
        {
            return @object is Hashtable;
        }
    }
}