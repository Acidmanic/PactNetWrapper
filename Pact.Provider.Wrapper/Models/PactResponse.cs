using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.Models
{
    public class PactResponse
    {
        public int Status { get; set; }
        
        public Dictionary<string, string> Headers { get; set; }
        
        public Hashtable Body { get; set; }
        
        public Dictionary<string,MatchingRule> MatchingRules { get; set; }
    }
}