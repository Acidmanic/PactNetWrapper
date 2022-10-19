
// ReSharper disable once CheckNamespace
namespace Pact.Provider.Wrapper.Models.Augment
{
    public static class InteractionInfoExtensions
    {
        
        public static InteractionInfo UpdateFrom(this InteractionInfo record, Models.Pact pact, Interaction interaction)
        {
            record.ConsumerName = pact.Consumer?.Name;
            record.ProviderName = pact.Provider?.Name;
            record.RequestMethod = interaction.Request?.Method;
            record.RequestPath = interaction.Request?.Path;
            record.ExpectedStatusCode = interaction.Response.Status;
            record.ProviderState = interaction.ProviderState;
            return record;
        }

        
    }
}