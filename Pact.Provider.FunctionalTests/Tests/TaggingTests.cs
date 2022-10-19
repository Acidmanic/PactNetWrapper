using System;
using Pact.Provider.Wrapper.IO;
using Pact.Provider.Wrapper.Models.Augment;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.FunctionalTests.Tests
{
    public class TaggingTests
    {
        public void Main()
        {
            var tagger = new ColumnDelimitedPublicationTagger();

            var pacts = new Json().LoadDirectory("../../../../Pacts");

            foreach (var pact in pacts)
            {
                foreach (var interaction in pact.Interactions)
                {
                    var info = new InteractionInfo().UpdateFrom(pact, interaction);

                    var tag = tagger.TagInteraction(info);

                    Console.WriteLine(tag);

                    tag = tagger.TagEndpoint(info);

                    Console.WriteLine(tag);

                    tag = tagger.TagService(info);

                    Console.WriteLine(tag);
                }
            }
        }
    }
}