using System.Net.Http;

namespace Pact.Provider.Wrapper.Models.Augment
{
    public class InteractionInfo
    {
        public string RequestPath { get; set; }

        public HttpMethod RequestMethod { get; set; }

        public string ProviderName { get; set; }

        public string ConsumerName { get; set; }
        
        public int ExpectedStatusCode { get; set; }
    }
}