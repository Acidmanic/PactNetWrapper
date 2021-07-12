using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort
{
    public class Matcher
    {

        public bool IsMatch(Dictionary<string, MatchingRule> ruleset,
            Dictionary<string,List<string>> expected,
            Dictionary<string,List<string>> actual,
            PactLogBuilder log)
        {
            bool match = true;
            
            var ruleFinder = new RuleFinder();
            
            foreach (var keyValuePair in expected)
            {
                var corresponding = Find(actual, keyValuePair.Key);
                
                if (corresponding.Key == null)
                {
                    log.NotFound(corresponding.Key);

                    match = false;
                }
                else
                {
                    var path = $"$.headers.{keyValuePair.Key}";

                    var expectedMatchingRule = ruleFinder.FindRule(path, ruleset);

                    foreach (var expectedValue in keyValuePair.Value)
                    {
                        if (!ContainsMatch(expectedMatchingRule, expectedValue, corresponding.Value))
                        {
                            match = false;

                            log.NotFound(path);
                        }
                    }   
                }
            }
            return match;
        }

        public bool ContainsMatch<TExpected, TActual>(MatchingRule rule, TExpected expected, IEnumerable<TActual> actuals)
        {
            foreach (var actual in actuals)
            {
                if (IsMatch(rule, expected, actual))
                {
                    return true;
                }
            }
            return false;
        }

        private KeyValuePair<string, List<string>> Find(Dictionary<string,List<string>> actual, string key)
        {
            throw new NotImplementedException();
        }

        public bool IsMatch(Dictionary<string, MatchingRule> ruleset, 
            Dictionary<string, object> expected,
            Dictionary<string, object> actual,
            PactLogBuilder log)
        {
            var finder = new RuleFinder();
            var match = true;
            
            foreach (var expectedKey in expected.Keys)
            {
                if (!actual.ContainsKey(expectedKey))
                {
                    match = false;

                    log.NotFound(expectedKey);
                }
                else
                {
                    var expectedValue = expected[expectedKey];
                    var actualValue = actual[expectedKey];
                    var rule = finder.FindRule(expectedKey, ruleset);

                    if (!IsMatch(rule, expectedValue, actualValue))
                    {
                        match = false;

                        log.Unmatched(expectedValue, actualValue, rule);
                        
                    }
                }
            }
            return match;
        }

        public bool IsMatch(MatchingRule rule, object expected, object actual)
        {
            if (rule.Match == "type")
            {
                return TypeMatch(expected, actual);
            }

            if (rule.Match == "regex")
            {
                return RegExMatch(actual,rule.Regex);
            }
            if (expected == null)
            {
                return actual == null;
            }
            return expected.Equals(actual);
        }

        private bool RegExMatch(object actual,string regex)
        {
            if (actual is string stringValue)
            {
                if (string.IsNullOrEmpty(regex))
                {
                    return true;
                }
                Regex r = new Regex(regex);
            
                return r.IsMatch(stringValue);    
            }
            return false;
        }

        private bool TypeMatch(object expected, object actual)
        {
            var expectedType = expected.GetEcmaType();
            
            var actualType  = actual.GetEcmaType();

            if (!expectedType.Equals(actualType))
            {
                return false;
            }
            if (EcmaTypes.EcmaTypes.Object.Equals(expectedType))
            {
                return actualType.ProtoType.Covers(expectedType.ProtoType);
            }
            return true;
        }

        
    }
}