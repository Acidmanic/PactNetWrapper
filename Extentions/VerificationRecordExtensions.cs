
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Models.Augment;

// ReSharper disable once CheckNamespace
namespace Pact.Provider.Wrapper.Verification
{
    public static class VerificationRecordExtensions
    {
        public static VerificationRecord UpdateFrom(this VerificationRecord record, Models.Pact pact, Interaction interaction)
        {
            record.Interaction = new InteractionInfo().UpdateFrom(pact,interaction);

            return record;
        }
        
        public static VerificationRecord UpdateFrom(this VerificationRecord record, PactnetVerificationResult result)
        {
            record.Exception = result.Exception;

            record.Logs = result.Logs;

            record.Success = result.Success;

            return record;
        }

        public static string DescribeInteraction(this VerificationRecord record)
        {
            return $"{record.Interaction.ProviderName}<>{record.Interaction.ConsumerName} :: " +
                   $"{record.Interaction.RequestMethod} > {record.Interaction.RequestPath}";
        }
    }
}