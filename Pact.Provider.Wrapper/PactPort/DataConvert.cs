using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Pact.Provider.Wrapper.PactPort
{
    public class DataConvert
    {
        

        public Dictionary<string, List<string>> Normalize(HttpResponseHeaders src)
        {
            var normalized = new Dictionary<string, List<string>>();

            var listedValues = src.ToList();

            foreach (var keyValuePair in listedValues)
            {
                normalized.Add(keyValuePair.Key, keyValuePair.Value.ToList());
            }

            return normalized;
        }

        public Dictionary<string, List<string>> Normalize(IEnumerable<KeyValuePair<string, string>> src)
        {
            var normalized = new Dictionary<string, List<string>>();

            foreach (var keyValuePair in src)
            {
                string[] values = keyValuePair.Value?.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

                values = values ?? new string[] { };

                if (!normalized.ContainsKey(keyValuePair.Key))
                {
                    normalized.Add(keyValuePair.Key, new List<string>());
                }

                normalized[keyValuePair.Key].AddRange(values);
            }

            return normalized;
        }

        
        
    }
}