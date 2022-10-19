using System.Collections.Generic;

namespace Pact.Provider.Wrapper.Models
{
    public class Pact
    {
        public Party Consumer { get; set; }
        
        public Party Provider { get; set; }
        
        public PactMetadata Metadata { get; set; }
        
        public List<Interaction> Interactions { get; set; }
        
        public BrockerApi _links { get; set; }
    }
}