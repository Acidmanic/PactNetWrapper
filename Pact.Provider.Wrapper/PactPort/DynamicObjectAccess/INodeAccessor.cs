using System;
using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public interface INodeAccessor
    {
        object GetChild(string key);

        ICollection<string> GetChildren();
        
        bool Supports(Type type);
        bool Supports(object @object);

        void Wrap(object data, Type type, KeyMaker keyMaker);
        
        string SeparatorFromParent { get; }
    }
}