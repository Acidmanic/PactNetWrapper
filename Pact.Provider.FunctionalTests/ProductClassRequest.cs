using System.Collections.Generic;

namespace Pact.Provider.FunctionalTests
{
    public class ProductClassRequest
    {
        public List<Property> CurrentFilter { get; set; }
        
        public string RequesterUserId { get; set; }
    }
}