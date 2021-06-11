using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

namespace Pact.Provider.Wrapper.Models
{
    public class PactRequest
    {
        public HttpMethod Method { get; set; }

        public string Path { get; set; }

        public Dictionary<string, string> Headers { get; set; }
        
        public string Query { get; set; }
        
        public Hashtable Body { get; set; }
    }
}