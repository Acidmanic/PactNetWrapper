using System;
using System.Collections;
using System.Collections.Generic;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort.RequestFilters
{
    public class RequestFilterApplier
    {
        //TODO: Use an abstraction and a factory to manage filter-appliers 
        public Interaction Apply(RequestFilter filter, Interaction interaction)
        {
            string headersKey = "$.headers.";
            string bodyKey = "$.body.";
            string queryKey = "$.query.";
            string pathKey = "$.path";

            var requestBody = interaction.Request.Body;

            var requestBodyData = new DynamicObjectAccess.FlatAccess(true).Flatten(requestBody, "");

            var requestQueryData = QueryToDictionary(interaction.Request.Query);
            
            if (filter.UrlMatcher.Matches(interaction.Request.Path))
            {
                if (filter.DataKey.StartsWith(headersKey))
                {
                    string dataKey = TrimStart(filter.DataKey, headersKey);

                    interaction = ApplyHeaderFilter(interaction, dataKey, filter.OverrideValue);
                }
                else if (filter.DataKey.StartsWith(bodyKey))
                {
                    string dataKey = TrimStart(filter.DataKey, bodyKey);

                    if (requestBodyData.ContainsKey(dataKey))
                    {
                        requestBodyData[dataKey] = filter.OverrideValue;
                    }
                }
                else if (filter.DataKey.StartsWith(queryKey))
                {
                    string dataKey = TrimStart(filter.DataKey, queryKey);

                    if (requestQueryData.ContainsKey(dataKey))
                    {
                        requestQueryData[dataKey] = filter.OverrideValue as string;
                    }
                }else if (filter.DataKey == pathKey)
                {
                    var path = filter.OverrideValue as string;

                    if (!string.IsNullOrEmpty(path))
                    {
                        interaction.Request.Path = path;
                    }
                }
            }

            interaction.Request.Body = requestBody;

            interaction.Request.Body = requestBody;

            interaction.Request.Query = DictionaryToQuery(requestQueryData);
            
            return interaction;
        }

        private string DictionaryToQuery(Dictionary<string, string> queryData)
        {
            var sep = "";

            string query = "";
            
            foreach (var keyValuePair in queryData)
            {
                query += sep + keyValuePair.Key;
                
                if (!string.IsNullOrEmpty(keyValuePair.Value))
                {
                    query += "=" + keyValuePair.Value;
                }

                sep = "&";
            }

            return query;
        }

        private Dictionary<string, string> QueryToDictionary(string query)
        {
            var queryData = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(query))
            {
                
                var segments = query.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < segments.Length; i++)
                {
                    var segment = segments[0];

                    var st = segment.IndexOf("=", StringComparison.Ordinal);

                    var key = segment;
                
                    string value = null;

                    if (st > -1)
                    {
                        key = segment.Substring(0, st);

                        value = segment.Substring(st, segment.Length - st);
                    }
                    queryData.Add(key,value);
                }
            }
            return queryData;
        }

        private Interaction ApplyHeaderFilter(Interaction interaction, string dataKey, object filterOverrideValue)
        {
            var key = dataKey.Trim().ToLower();

            string value = filterOverrideValue.ToString();

            RemoveAll(key, interaction.Request.Headers);

            interaction.Request.Headers.Add(key, value);

            return interaction;
        }

        private void RemoveAll(string key, Dictionary<string, string> requestHeaders)
        {
            var removing = new List<string>();

            foreach (var headerKey in requestHeaders.Keys)
            {
                if (headerKey.ToLower() == key)
                {
                    removing.Add(headerKey);
                }
            }

            foreach (var headerKey in removing)
            {
                if (requestHeaders.ContainsKey(headerKey))
                {
                    requestHeaders.Remove(headerKey);
                }
            }
        }
        
        private string TrimStart(string main, string starter)
        {
            if (main.StartsWith(starter))
            {
                return main.Substring(starter.Length, main.Length - starter.Length);
            }

            return main;
        }
    }
}