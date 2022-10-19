using System;
using Pact.Provider.Wrapper.UrlUtilities;

namespace Pact.Provider.Wrapper.Unit
{
    
    
    public abstract class EndpointMatchAttributeBase:Attribute
    {
        
        
        public IUrlMatcher AddingUrls { get; protected set; }
        
        public IUrlMatcher RemovingUrls { get; protected set; }
    }
}