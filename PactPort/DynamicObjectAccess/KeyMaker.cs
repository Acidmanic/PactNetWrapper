using System;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public class KeyMaker
    {
        
        private  bool CamelCase { get; set; }

        public KeyMaker()
        {
            CamelCase = false;
        }

        public KeyMaker(bool camelCase)
        {
            CamelCase = camelCase;
        }
        
        public string MakePath(string prefix, string key, string separator)
        {
            var casedKey = SetCase(key);

            return String.IsNullOrEmpty(prefix) ? casedKey : (prefix + separator + casedKey);
        }
        
        public string SetCase(string id)
        {
            if (string.IsNullOrEmpty(id) || id.Length == 1)
            {
                return id;
            }

            char firstChar = id.ToCharArray()[0];

            string rest = id.Substring(1, id.Length - 1);

            if (CamelCase)
            {
                return char.ToLower(firstChar) + rest;
            }
            else
            {
                return char.ToUpper(firstChar) + rest;
            }
        }
    }
}