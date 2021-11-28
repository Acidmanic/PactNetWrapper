using System;
using System.Net;
using System.Text;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort
{
    public class PactLogBuilder
    {
        private readonly StringBuilder _content;

        public PactLogBuilder()
        {
            _content = new StringBuilder();
        }

        public PactLogBuilder at(string fieldKey)
        {
            _content.Append("At: ").Append(fieldKey).Append("\n");

            return this;
        }

        public PactLogBuilder Unmatched(string expected, string actual)
        {
            Unmatched(expected, actual, "");

            return this;
        }
        
        public PactLogBuilder Unmatched(string expected, string actual,string expectationCaption)
        {
            _content.Append($"Expected {expectationCaption}: {expected}, But Received {actual}.\n");

            return this;
        }
        
        public PactLogBuilder Unmatched(object expected, object actual,string expectationCaption)
        {
            _content.Append($"Expected {expectationCaption}: {expected}, But Received {actual}.\n");

            return this;
        }
        
        public PactLogBuilder Unmatched(int expected, HttpStatusCode actual)
        {
            Unmatched(Convert.ToString(expected), Convert.ToString((int)actual), "Status code");

            return this;
        }
        
        
        public PactLogBuilder Unmatched(object expected, object actual,MatchingRule rule)
        {
            string expectationCaption;
            
            if (rule.Match == "regex")
            {
                expectationCaption = $"A string value matching: '{rule.Regex}'";
            }
            else if(rule.Match=="type")
            {
                expectationCaption = $"A value of a type compatible with: '{expected.GetType().Name}'";
            }
            else
            {
                expectationCaption = "";
            }

            Unmatched(expected, actual, expectationCaption);
            
            return this;
        }
        
        public PactLogBuilder NotFound(string key)
        {
            Unmatched(key, "Nothing");

            return this;
        }

        public override string ToString()
        {
            return _content.ToString();
        }
    }
}