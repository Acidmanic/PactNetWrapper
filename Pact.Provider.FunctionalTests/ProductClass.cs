using System.Collections.Generic;

namespace Pact.Provider.FunctionalTests
{
    public class ProductClass
    {
        public long Id { get; set; }
        
        public List<Property> Properties { get; set; }
        
        public string Description { get; set; }
    }
}