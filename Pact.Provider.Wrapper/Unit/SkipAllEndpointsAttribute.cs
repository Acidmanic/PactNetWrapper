using System;

namespace Pact.Provider.Wrapper.Unit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SkipAllEndpointsAttribute:Attribute
    {

    }
}