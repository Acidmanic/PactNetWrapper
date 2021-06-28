namespace Pact.Provider.Wrapper.PactPort.RequestFilters
{
    public class RequestFilter
    {
        public string RequestPath { get; set; }

        public string DataKey { get; set; }
        
        public object OverrideValue { get; set; } 
        
    }
}