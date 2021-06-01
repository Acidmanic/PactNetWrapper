using System;

namespace Pact.Provider.Wrapper.Unit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EndpointAttribute : Attribute
    {
        private string _requestPath;

        public EndpointAttribute(string requestPath)
        {
            _requestPath = requestPath;
        }

        public string RequestPath => _requestPath;
    }
}