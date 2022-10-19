using System;
using Pact.Provider.Wrapper.UrlUtilities;

namespace Pact.Provider.Wrapper.Unit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EndpointAttribute : EndpointMatchAttributeBase
    {
        public EndpointAttribute(string requestPathPattern,bool caseSensitive = true, bool acceptChildren = false)
        {
            RemovingUrls = new NoneUrlMatcher();
            
            AddingUrls = new BySegmentUrlMatcher(requestPathPattern,caseSensitive,acceptChildren);
        }
    }
}