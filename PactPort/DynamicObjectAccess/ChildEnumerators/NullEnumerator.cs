using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators
{
    public class NullEnumerator:IChildEnumerator
    {
        public Dictionary<string, object> Enumerate(object data)
        {
            return new Dictionary<string, object>();
        }

        public bool Supports(object data)
        {
            // Prevent appearing in factory result automatically 
            return false;
        }

        public string SeparatorFromParent => ".";
    }
}