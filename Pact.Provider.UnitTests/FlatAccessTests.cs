using System.Collections.Generic;
using Newtonsoft.Json;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.PactPort;
using Pact.Provider.Wrapper.PactPort.DynamicObjectAccess;
using Xunit;

namespace Pact.Provider.UnitTests
{
    public class FlatAccessTests
    {
        private class SimpleTreeWithArray
        {
            public List<string> Items { get; set; }
        }

        private class Data
        {
            public InnerField InnerField { get; set; }
        }

        private class InnerField
        {
            public string Property { get; set; }
        }

        [Fact]
        public void DynamicObjectAccessShouldFlattenTheObjectTheWayMatchesMatchesThem()
        {
            var sut = new FlatAccess(true);

            var exObj = CreateSampleTree();
            var acObj = CreateSampleTree();

            var firstFlat = sut.Flatten(exObj, "$");
            var secondFlat = sut.Flatten(acObj, "$");

            var result = new Matcher().IsMatch(new MatchingRule {Match = "exact"}, firstFlat, secondFlat);

            Assert.False(result);
        }

        [Fact]
        public void DynamicObjectAccessShouldFlattenAnArray()
        {
            var sut = new FlatAccess(true);

            var arrayToFlatten = new List<string>
            {
                "First",
                "Second"
            };

            var flatten = sut.Flatten(arrayToFlatten, "$");

            Assert.Equal(2, flatten.Count);

            var expectedValues = new Dictionary<string, string>
            {
                {"$[0]", "First"},
                {"$[1]", "Second"}
            };

            foreach (var key in expectedValues.Keys)
            {
                Assert.Contains(key, flatten.Keys);

                Assert.Equal(expectedValues[key], flatten[key]);
            }
        }

        private SimpleTreeWithArray CreateSampleTree()
        {
            return new SimpleTreeWithArray
            {
                Items = new List<string>
                {
                    "First",
                    "Second",
                    "Third"
                }
            };
        }

        [Fact]
        public void DynamicObjectAccessShouldFlattenNestedTypeObjectCorrectly()
        {
            var sut = new FlatAccess(true);
            
            var data = new Data
            {
                InnerField = new InnerField
                {
                    Property = "property"
                }
            };

            var result = sut.Flatten(data, "$");
            
            Assert.Single(result);

            Assert.Contains("$.innerField.property",result.Keys);
            
            Assert.Equal("property",result["$.innerField.property"]);
        }

        [Fact]
        public void DynamicObjectAccessShouldFlattenJsonMap()
        {
            var json = DynamicObjectAccessResources.ItemsSearchResponse;

            var data = JsonConvert.DeserializeObject(json);
            
            var sut = new FlatAccess();

            var result = sut.Flatten(data,"$");
            
            Assert.Equal(28,result.Count);
            
        }


    }
}