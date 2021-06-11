using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Pact.Provider.Wrapper.PactPort
{
    public class DataConvert
    {
        
        
        public Dictionary<string, object> Flatten(Hashtable data, string prefix)
        {
            var result = new Dictionary<string, object>();

            Flatten(prefix, data, result);

            return result;
        }

        private void Flatten(string prefix, Hashtable data, Dictionary<string, object> result)
        {
            foreach (DictionaryEntry entry in data)
            {
                if (entry.Value is Hashtable)
                {
                    Flatten(prefix + "." + entry.Key, entry.Value as Hashtable, result);
                }
                else
                {
                    result.Add(prefix + "." + (string) entry.Key, entry.Value);
                }
            }
        }

        public Dictionary<string, List<string>> Normalize(HttpResponseHeaders src)
        {
            var normalized = new Dictionary<string, List<string>>();
            
            var listedValues = src.ToList();

            foreach (var keyValuePair in listedValues)
            {
                normalized.Add(keyValuePair.Key,keyValuePair.Value.ToList());
            }

            return normalized;
        }

        public Dictionary<string, List<string>> Normalize(IEnumerable<KeyValuePair<string, string>> src)
        {
            var normalized = new Dictionary<string, List<string>>();
                
            foreach (var keyValuePair in src)
            {
                string[] values = keyValuePair.Value?.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);

                values = values ?? new string[] { };

                if (!normalized.ContainsKey(keyValuePair.Key))
                {
                    normalized.Add(keyValuePair.Key,new List<string>());
                }
                normalized[keyValuePair.Key].AddRange(values);
            }
            return normalized;
        }

        public Dictionary<string, object> Flatten(object data, string prefix)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            Flatten(data, prefix, result);

            return result;
        }

        public void Flatten(object data, string prefix, Dictionary<string, object> result)
        {
            if (data != null)
            {
                var type = data.GetType();

                if (type.IsPrimitive)
                {
                    result.Add(prefix,data);
                }
                else
                {
                    var properties = type.GetProperties();

                    foreach (var property in properties)
                    {
                        if (property.CanWrite && property.CanRead)
                        {
                            try
                            {
                                var value = property.GetValue(data);
                            
                                Flatten(value, property.Name, result);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                }
            }
        }
        
    }
}