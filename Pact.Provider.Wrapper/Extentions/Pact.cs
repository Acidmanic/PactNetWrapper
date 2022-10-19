using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Pact.Provider.Wrapper.Models
{
    public static class PactExtensions
    {
        public static List<Pact> SplitByInteractions(this Pact pact)
        {
            List<Pact> pacts = new List<Pact>();

            foreach (var interaction in pact.Interactions)
            {
                var instance = CopyBase(pact);
                
                instance.Interactions.Add(interaction);
                
                pacts.Add(instance);
            }
            return pacts;
        }

        public static Dictionary<string, Pact> SplitByEndpoint(this Pact pact, bool caseSensitive = false)
        {
            Dictionary<string, Pact> pactsByEndpoint = new Dictionary<string, Pact>();
            
            foreach (var interaction in pact.Interactions)
            {
                var key = interaction.Request.Path;

                if (!caseSensitive)
                {
                    key = key.ToLower();
                    
                }
                if (!pactsByEndpoint.ContainsKey(key))
                {
                    var value = CopyBase(pact);
                    
                    pactsByEndpoint.Add(key,value);
                }
                Pact instance = pactsByEndpoint[key];
                
                instance.Interactions.Add(interaction);
            }
            return pactsByEndpoint;
        }

        private static Pact CopyBase(Pact pact)
        {
            Pact p = new Pact();

            p.Consumer = new Party {Name = pact.Consumer?.Name};

            p.Provider = new Party {Name = pact.Provider?.Name};
            
            p.Metadata = new PactMetadata
            {
                PactSpecification = new Specification
                {
                    Version = pact.Metadata?.PactSpecification?.Version
                }
            };
            p.Interactions = new List<Interaction>();

            p._links = pact._links?.Clone();
            
            return p;
        }
    }
}