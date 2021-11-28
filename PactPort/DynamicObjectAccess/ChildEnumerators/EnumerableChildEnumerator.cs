using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators
{
    public class EnumerableChildEnumerator : TypeSpecificChildEnumerator<IEnumerable>
    {
        protected override void EnumerateChildrenInto(IEnumerable data, Dictionary<string, object> result)
        {
            int index = 0;

            foreach (var item in data)
            {
                var key = "[" + index.ToString() + "]";

                result.Add(key, item);

                index += 1;
            }
        }
        
        public override string SeparatorFromParent => "";
    }
}