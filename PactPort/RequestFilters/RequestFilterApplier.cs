using System.Collections;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort.RequestFilters
{
    public class RequestFilterApplier
    {
        public Interaction Apply(RequestFilter filter, Interaction interaction)
        {
            string headersKey = "$.headers.";
            string bodyKey = "$.body.";
            
            var requestBody = interaction.Request.Body;
            
            var  requestBodyData = new DynamicObjectAccess(true).Flatten(requestBody,"");

            if (RequestPathMatch(filter.RequestPath, interaction.Request.Path))
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
            }
            
            requestBody = new DynamicObjectAccess(true).LoadInto(requestBody,requestBodyData);

            interaction.Request.Body = requestBody;

            return interaction;
        }

        private Interaction ApplyHeaderFilter(Interaction interaction, string dataKey, object filterOverrideValue)
        {
            var key = dataKey.Trim();

            string value = filterOverrideValue.ToString();

            if (interaction.Request.Headers.ContainsKey(key))
            {
                interaction.Request.Headers.Remove(key);
            }

            interaction.Request.Headers.Add(key, value);

            return interaction;
        }


        private string TrimStart(string main, string starter)
        {
            if (main.StartsWith(starter))
            {
                return main.Substring(starter.Length, main.Length - starter.Length);
            }

            return main;
        }

        private bool RequestPathMatch(string parentPath, string requestPath)
        {
            if (string.IsNullOrEmpty(parentPath) || parentPath == "/")
            {
                return true;
            }

            if (!parentPath.EndsWith("/"))
            {
                parentPath += "/";
            }

            if (!requestPath.EndsWith("/"))
            {
                requestPath += "/";
            }

            return requestPath.StartsWith(parentPath);
        }
    }
}