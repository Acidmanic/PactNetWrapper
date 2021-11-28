using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators
{
    public class HashtableChildEnumerator:TypeSpecificChildEnumerator<Hashtable>
    {
        protected override void EnumerateChildrenInto(Hashtable data, Dictionary<string, object> result)
        {
            foreach (DictionaryEntry entry in data)
            {
                if (entry.Key is string key)
                {
                    result.Add(key,entry.Value);
                }
            }
        }
    }
}