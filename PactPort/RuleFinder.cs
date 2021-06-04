using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort
{
    public class RuleFinder
    {


        public MatchingRule FindRule(string path,Dictionary<string,MatchingRule> ruleSet)
        {
            while (!string.IsNullOrEmpty(path))
            {
                if (ruleSet.ContainsKey(path))
                {
                    return ruleSet[path];
                }
                path = Parent(path);
            }
            return new MatchingRule(){Match = "exact"};
        }

        private string Parent(string path)
        {
            int st = path.LastIndexOf(".", StringComparison.Ordinal);
            
            if (st == -1)
            {
                return "";
            }
            return path.Substring(0, st);
        }
    }
}