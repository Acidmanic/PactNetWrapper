using System.Collections.Generic;
using Pact.Provider.Wrapper.UrlUtilities;

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
                UrlMatcher = new BatchUrlMatcher(),
                DataKey = "",
                OverrideValue = null
            };

            _containsNotDelivered = true;

            return this;
        }
        
        public RequestFilterCollectionBuilder WithRequestPathUnder(string path,bool caseSensitive = true)
        {
            if (_filter.UrlMatcher is BatchUrlMatcher batchUrlMatcher)
            {
                batchUrlMatcher.Add(new BySegmentUrlMatcher(path, caseSensitive, true));
            }
            
            return this;
        }
        
        public RequestFilterCollectionBuilder WithRequestPath(string path,bool caseSensitive = true)
        {
            if (_filter.UrlMatcher is BatchUrlMatcher batchUrlMatcher)
            {
                batchUrlMatcher.Add(new BySegmentUrlMatcher(path, caseSensitive, false));
            }
            
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
                if (_filter.UrlMatcher is BatchUrlMatcher batchUrlMatcher)
                {
                    if (batchUrlMatcher.IsEmpty())
                    {
                        _filter.UrlMatcher = new AllUrlMatcher();
                    }
                }
                
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