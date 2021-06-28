using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.RequestFilters
{
    public class RequestFilterCollectionBuilder
    {
        private bool _containsNotDelivered = false;
        private RequestFilter _filter;
        private readonly List<RequestFilter> _filters  = new List<RequestFilter>();

        
        public RequestFilterCollectionBuilder Add()
        {
            CheckDelivery();
            
            _filter = new RequestFilter
            {
                RequestPath = "",
                DataKey = "",
                OverrideValue = null
            };

            _containsNotDelivered = true;

            return this;
        }
        
        public RequestFilterCollectionBuilder WithRequestPathUnder(string path)
        {
            _filter.RequestPath = path;

            return this;
        }


        public RequestFilterCollectionBuilder Put(object value)
        {
            _filter.OverrideValue = value;

            return this;
        }

        public RequestFilterCollectionBuilder At(string dataPath)
        {
            _filter.DataKey = dataPath;

            return this;
        }

        private void CheckDelivery()
        {
            if (_containsNotDelivered)
            {
                _filters.Add(_filter);
                
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