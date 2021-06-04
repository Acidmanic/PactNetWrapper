using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort
{
    public class Matcher
    {


        public bool IsMatch(Dictionary<string, MatchingRule> ruleset, 
            Dictionary<string, object> expected,
            Dictionary<string, object> actual,
            StringBuilder log)
        {
            var finder = new RuleFinder();
            var match = true;
            
            foreach (var expectedKey in expected.Keys)
            {
                if (!actual.ContainsKey(expectedKey))
                {
                    match = false;

                    log.Append($"Unable to find key: {expectedKey}.\n");
                }
                else
                {
                    var expectedValue = expected[expectedKey];
                    var actualValue = actual[expectedKey];
                    var rule = finder.FindRule(expectedKey, ruleset);

                    if (!IsMatch(rule, expectedValue, actualValue))
                    {
                        match = false;

                        var expectedMessage = (rule.Match == "regex") ? rule.Regex : expectedValue;
                        
                        log.Append($"Expected to find value matching {rule.Match} " +
                                   $"with {expectedMessage}, " +
                                   $"but found {actualValue}\n");
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
            if ((expected is string && actual is string)
                || (IsInteger(expected) && IsInteger(actual))
                || (IsFloatingPoint(expected) && IsFloatingPoint(actual))
                || (expected is bool && actual is bool)
                || (expected is char && actual is char))
            {
                return true;
            }
            return expected.GetType() == actual.GetType();
        }

        private bool IsFloatingPoint(object value)
        {
            return value is float || value is double ||
                   value is decimal;
        }

        private bool IsInteger(object value)
        {
            return value is int || value is long || value is short
                   || value is byte || value is uint || value is ulong
                   || value is ushort;
        }
    }
}