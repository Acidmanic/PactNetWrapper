using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.IO;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Models.Augment;
using Pact.Provider.Wrapper.PactPort;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.FunctionalTests.Tests
{
    public class MatcherTest
    {
        public void Main()
        {

            var matcher = new Matcher();
            var log = new PactLogBuilder();
            
            var expected = new Dictionary<string,object>
            {
                {"a.b.c1",12},
                {"a.b.c2","13"},
                {"a.b.c3","ghozmit"},
            };
            
            var actual = new Dictionary<string,object>{
                {"a.b.c1",14},
                {"a.b.c2","15"}
            };

            matcher.IsMatch(new Dictionary<string, MatchingRule>(),  expected, actual, log);

            Console.WriteLine(log.ToString());
        }
    }
}