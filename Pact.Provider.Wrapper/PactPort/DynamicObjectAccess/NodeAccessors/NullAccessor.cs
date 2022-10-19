using System;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.NodeAccessors
{
    public class NullAccessor : INodeAccessor
    {
        public object GetChild(string key)
        {
            return null;
        }


        public ICollection<string> GetChildren()
        {
            return new List<string>();
        }


        public bool Supports(Type type)
        {
            //Prevent auto appearing in Reflection based factories
            return false;
        }

        public bool Supports(object @object)
        {
            //Prevent auto appearing in Reflection based factories
            return false;
        }

        public void Wrap(object data, Type type, KeyMaker keyMaker)
        {
        }

        public string SeparatorFromParent => ".";
    }
}