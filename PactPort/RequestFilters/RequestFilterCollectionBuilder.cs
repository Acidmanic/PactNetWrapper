using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.RequestFilters
{
    public class RequestFilterCollectionBuilder
    {
        private bool _containsNotDelivered = false;
        
        private readonly List<RequestFilter> _filters  = new List<RequestFilter>();

        private RequestFilterBuilder _requestFilterBuilder = null;
        
        
        
        public RequestFilterBuilder Add()
        {
            CheckDelivery();
            
            _requestFilterBuilder = new RequestFilterBuilder();

            _containsNotDelivered = true;

            return _requestFilterBuilder;
        }

        private void CheckDelivery()
        {
            if (_containsNotDelivered)
            {
                var filter = _requestFilterBuilder.Build();
                
                _filters.Add(filter);
                
                _containsNotDelivered = false;
            }
        }


        public List<RequestFilter> Build()
        {
            CheckDelivery();
            
            return new List<RequestFilter>(_filters);
        }
        
    }
}