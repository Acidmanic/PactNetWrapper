using System;

namespace Pact.Provider.Wrapper.Unit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SkipEndpointAttribute:Attribute
    {
        private string _requestPath;

        public SkipEndpointAttribute(string requestPath)
        {
            _requestPath = requestPath;
        }

        public string RequestPath => _requestPath;
    }
}