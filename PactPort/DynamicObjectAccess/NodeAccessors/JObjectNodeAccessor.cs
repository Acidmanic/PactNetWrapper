using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.NodeAccessors
{
    public class JObjectNodeAccessor : NodeAccessorBase<JObject>
    {
        public override object GetChild(string key)
        {
            if (_data is JObject jMe)
            {
                foreach (var token in jMe)
                {
                    if (token.Key.Equals(key))
                    {
                        return token.Value;
                    }
                }
            }
            return null;
        }

        public override ICollection<string> GetChildren()
        {
            var result = new List<string>();

            if (_data is JObject jMe)
            {
                foreach (var token in jMe)
                {
                    result.Add(token.Key);
                }
            }

            return result;
        }

        public override bool Supports(Type type)
        {
            return IsDerivedFrom<JObject>(type);
        }

        public override bool Supports(object @object)
        {
            return @object is JObject;
        }
    }
}