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

            // Result
            var selected = new Dictionary<string, T>();
            
            var endpointMatchers = new AttributeHelper().DeliveredAttributes<EndpointMatchAttributeBase>();

            // Add-1) Add endpoints per attribute

            foreach (var keyValuePair in existing)
            {
                if (ShouldAdd(keyValuePair.Key, endpointMatchers))
                {
                    if (!selected.ContainsKey(keyValuePair.Key))
                    {
                        selected.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }

            // Add-All) If no endpoints has been added yet, add all endpoints for the case that no Adding Att 
            // has been provided

            if (!endpointMatchers.Any(ep => ep is EndpointAttribute))
            {
                foreach (var keyValuePair in existing)
                {
                    selected.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            // SKIP) Remove Skipped endpoints per attribute

            var removingKeys = new List<string>();

            foreach (var keyValuePair in selected)
            {
                var key = keyValuePair.Key.NormalizeHttpUri().ToLower();

                if (ShouldRemove(key, endpointMatchers))
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

        private bool ShouldAdd(string url, List<EndpointMatchAttributeBase> endpointMatchers)
        {
            foreach (var matcher in endpointMatchers)
            {
                if (matcher.AddingUrls.Matches(url))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ShouldRemove(string url, List<EndpointMatchAttributeBase> endpointMatchers)
        {
            foreach (var matcher in endpointMatchers)
            {
                if (matcher.RemovingUrls.Matches(url))
                {
                    return true;
                }
            }

            return false;
        }

        // private bool IsLabeled(string endpoint, List<string> labeledEndpoints, bool acceptChildren = false)
        // {
        //     var key = endpoint.NormalizeHttpUri().ToLower();
        //
        //     return acceptChildren
        //         ? ContainsKeyWhichStartsGivenKey(labeledEndpoints, key)
        //         : labeledEndpoints.Contains(key);
        // }
        //
        // private bool ContainsKeyWhichStartsGivenKey(List<string> labels, string givenKey)
        // {
        //     foreach (var label in labels)
        //     {
        //         if (givenKey.StartsWith(label))
        //         {
        //             return true;
        //         }
        //     }
        //
        //     return false;
        // }
    }
}