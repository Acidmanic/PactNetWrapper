
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Models.Augment;

namespace Pact.Provider.Wrapper.Verification
{
    public interface IPublicationTagger
    {
        string TagInteraction(InteractionInfo interaction);

        string TagEndpoint(InteractionInfo interaction);

        string TagService(InteractionInfo interaction);
    }
}