using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Models.Augment;

namespace Pact.Provider.Wrapper.Verification
{
    public class ColumnDelimitedInteractionTagger : IInteractionTagger
    {
        public string Tag(InteractionInfo interaction)
        {
            string tag = interaction.RequestPath?.ToLower();

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