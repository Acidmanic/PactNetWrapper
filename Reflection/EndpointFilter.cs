using System;
using System.Collections.Generic;
using System.Linq;
using Pact.Provider.Wrapper.Unit;

namespace Pact.Provider.Wrapper.Reflection
{
    /// <summary>
    /// This class Filters entries from a dictionary mapped on endpoint. based on available
    /// endpoint-selecting attributes.
    /// </summary>
    public class EndpointFilter
    {
        public Dictionary<string, T> Filter<T>(Dictionary<string, T> existing)
        {
            var skipAll = new AttributeHelper().DeliveredAttributes<SkipAllEndpointsAttribute>();

            if (skipAll.Count > 0)
            {
                return new Dictionary<string, T>();
            }

            var addEndpoints =
                new AttributeHelper().DeliveredAttributes<EndpointAttribute>()
                    .Select(ep => ep.RequestPath.NormalizeHttpUri().ToLower()).ToList();

            var selected = new Dictionary<string, T>();

            foreach (var keyValuePair in existing)
            {
                if (addEndpoints.Count == 0 || IsLabeled(keyValuePair.Key, addEndpoints))
                {
                    selected.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            var skipEndpoints = new AttributeHelper().DeliveredAttributes<SkipEndpointAttribute>()
                .Select(ep => ep.RequestPath.NormalizeHttpUri().ToLower()).ToList();

            var removingKeys = new List<string>();

            foreach (var keyValuePair in selected)
            {
                var key = keyValuePair.Key.NormalizeHttpUri().ToLower();

                if (skipEndpoints.Contains(key))
                {
                    removingKeys.Add(keyValuePair.Key);
                }
            }

            foreach (var removingKey in removingKeys)
            {
                selected.Remove(removingKey);
            }

            return selected;
        }

        private bool IsLabeled(string endpoint, List<string> labeledEndpoints)
        {
            var key = endpoint.NormalizeHttpUri().ToLower();

            return labeledEndpoints.Count == 0 || labeledEndpoints.Contains(key);
        }
    }
}