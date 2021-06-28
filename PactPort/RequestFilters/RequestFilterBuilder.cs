using System;
using System.Linq.Expressions;

namespace Pact.Provider.Wrapper.PactPort.RequestFilters
{
    public class RequestFilterBuilder
    {

        private readonly RequestFilter _filter;

        public RequestFilterBuilder()
        {
            _filter = new RequestFilter
            {
                RequestPath = "",
                DataKey = "",
                OverrideValue = null
            };
        }


        public RequestFilterBuilder WithRequestPathUnder(string path)
        {
            _filter.RequestPath = path;

            return this;
        }


        public RequestFilterBuilder Put(object value)
        {
            _filter.OverrideValue = value;

            return this;
        }

        public RequestFilterBuilder At(string dataPath)
        {
            _filter.DataKey = dataPath;

            return this;
        }

        public RequestFilter Build()
        {
            return _filter;
        }
    }
}