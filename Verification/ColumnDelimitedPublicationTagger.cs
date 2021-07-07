using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Models.Augment;

namespace Pact.Provider.Wrapper.Verification
{
    public class ColumnDelimitedPublicationTagger : IPublicationTagger
    {
        public string TagInteraction(InteractionInfo interaction)
        {
            string tag = TagUriPath(interaction.RequestPath?.ToLower());

            var stateChars = interaction.ProviderState.ToCharArray();

            tag += ":" + interaction.RequestMethod.Method.ToLower() + ":";
            
            foreach (char c in stateChars)
            {
                if (char.IsDigit(c) || char.IsLetter(c))
                {
                    tag += c;
                }
            }

            return tag;
        }

        public string TagEndpoint(InteractionInfo interaction)
        {
            return TagUriPath(interaction.RequestPath?.ToLower());
        }


        private string TagUriPath(string uriPath)
        {
            string tag = uriPath?.ToLower();

            if (string.IsNullOrEmpty(tag))
            {
                return ":";
            }

            tag = tag.Replace("/", ":");

            tag = tag.Replace("\\", ":");

            while (tag.StartsWith(":"))
            {
                tag = tag.Substring(1, tag.Length - 1);
            }

            while (tag.EndsWith(":"))
            {
                tag = tag.Substring(0, tag.Length - 1);
            }

            return tag;
        }
    }
}