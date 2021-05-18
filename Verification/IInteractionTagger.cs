
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Models.Augment;

namespace Pact.Provider.Wrapper.Verification
{
    public interface IInteractionTagger
    {
        string Tag(InteractionInfo interaction);
    }
}