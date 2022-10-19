using System;
using Pact.Provider.Wrapper.UrlUtilities;

namespace Pact.Provider.Wrapper.Unit
{
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SkipEndpointAttribute : EndpointMatchAttributeBase
    {
        public SkipEndpointAttribute(string requestPathPattern,bool caseSensitive = true, bool acceptChildren = false)
        {
            RemovingUrls = new BySegmentUrlMatcher(requestPathPattern,caseSensitive,acceptChildren);
            
            AddingUrls = new NoneUrlMatcher();
        }
    }
}