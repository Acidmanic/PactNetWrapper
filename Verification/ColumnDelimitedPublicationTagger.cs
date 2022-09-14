using System;
using Pact.Provider.Wrapper.Models.Augment;

namespace Pact.Provider.Wrapper.Verification
{
    public class ColumnDelimitedPublicationTagger : IPublicationTagger
    {
        public string TagInteraction(InteractionInfo interaction)
        {
            string tag = TagUriPath(interaction.RequestPath?.ToLower());

            var stateChars = interaction.ProviderState.ToLower().ToCharArray();

            tag += ":" + interaction.RequestMethod.Method.ToLower() + ":";

            foreach (char c in stateChars)
            {
                if (char.IsDigit(c) || char.IsLetter(c))
                {
                    tag += c;
                }
            }

            return "interaction:" + tag;
        }

        public string TagEndpoint(InteractionInfo interaction)
        {
            return "endpoint:" + TagUriPath(interaction.RequestPath?.ToLower());
        }

        public string TagService(InteractionInfo interaction)
        {
            if (!string.IsNullOrEmpty(interaction.RequestPath))
            {
                string[] segments = interaction.RequestPath.Split(new char[] {'/'},
                    StringSplitOptions.RemoveEmptyEntries);

                if (segments.Length > 0)
                {
                    if (!string.IsNullOrEmpty(segments[0]))
                    {
                        return "service:" +segments[0].ToLower();
                    }
                }
            }

            return "service:root-resource";
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