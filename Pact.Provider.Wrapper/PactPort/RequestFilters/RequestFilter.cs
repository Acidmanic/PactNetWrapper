using Pact.Provider.Wrapper.UrlUtilities;

namespace Pact.Provider.Wrapper.PactPort.RequestFilters
{
    public class RequestFilter
    {
        public IUrlMatcher UrlMatcher { get; set; }

        public string DataKey { get; set; }
        
        public object OverrideValue { get; set; } 
        
    }
}