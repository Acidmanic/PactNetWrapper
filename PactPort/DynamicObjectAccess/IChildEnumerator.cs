using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public interface IChildEnumerator
    {
        Dictionary<string, object> Enumerate(object data);

        bool Supports(object data);
        
        string SeparatorFromParent { get;  }
    }
}