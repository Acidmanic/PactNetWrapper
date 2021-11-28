using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators
{
    public abstract class TypeSpecificChildEnumerator<TData>:IChildEnumerator
    {
        public Dictionary<string, object> Enumerate(object data)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (data is TData castedData)
            {
                EnumerateChildrenInto(castedData, result);
            }

            return result;
        }

        protected abstract void EnumerateChildrenInto(TData data, Dictionary<string, object> result);

        public bool Supports(object data)
        {
            return data is TData;
        }
    }
}