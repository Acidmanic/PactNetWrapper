namespace Pact.Provider.Wrapper.Models
{
    public class Interaction
    {
        public string Description { get; set; }
        
        public string ProviderState { get; set; }
        
        public PactRequest Request { get; set; }
        
        public PactResponse Response { get; set; }
        
        
    }
}