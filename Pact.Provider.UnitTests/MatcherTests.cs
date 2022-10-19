using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.PactPort;
using Xunit;

namespace Pact.Provider.UnitTests
{
    public class MatcherTests
    {

        private class SimpleTreeWithArray
        {
            public List<string> Items { get; set; }
        }
        
        [Fact]
        public void MatcherShouldFailIdenticalDataWithNoneFlatterenedArrays()
        {
            var sut = new Matcher();

            var exObj = CreateSampleTree();
            var acObj = CreateSampleTree();
            
            var result = sut.IsMatch(new MatchingRule{Match = "exact"}, exObj, acObj);
            
            Assert.False(result);
        }


        private enum Numbers
        {
            Zero=0,
            One=1,
            Two=2,
            Three=3
        }

        private class ModelWithEnum
        {
            public string Name { get; set; }
            public Numbers Number { get; set; }
        }
        
        [Fact]
        public void MatcherMustMatchEnumeratorsAndIntegers()
        {
            var sut = new Matcher();

            var expected = new 
            {
                Name="Mani",
                Number = 1
            };
            
            var actual = new 
            {
                Name="Mani",
                Number = Numbers.One
            };
            
            var result = sut.IsMatch(new MatchingRule{Match = "exact"}, expected, actual);
            
            Assert.False(result);
            
            result = sut.IsMatch(new MatchingRule{Match = "exact"}, actual, expected);
            
            Assert.False(result);
        }
        
        [Fact]
        public void MatcherMustMatchEnumeratorAndIntegerInClass()
        {
            var sut = new Matcher();

            var expected = new 
            {
                Name="Mani",
                Number = 1
            };
            
            var actual = new ModelWithEnum()
            {
                Name="Mani",
                Number = Numbers.One
            };
            
            var result = sut.IsMatch(new MatchingRule{Match = "exact"}, expected, actual);
            
            Assert.False(result);
          
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
    }
}
