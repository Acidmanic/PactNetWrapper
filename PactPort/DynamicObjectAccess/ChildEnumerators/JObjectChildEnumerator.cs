using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators
{
    public class JObjectChildEnumerator:TypeSpecificChildEnumerator<JObject>
    {
        protected override void EnumerateChildrenInto(JObject data, Dictionary<string, object> result)
        {
            foreach (var token in data)
            {
                result.Add(token.Key,token.Value);
            }
        }
    }
}