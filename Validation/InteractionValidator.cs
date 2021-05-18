using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.Validation
{
    public class InteractionValidator:ModelValidatorBase<Interaction>
    {
        public InteractionValidator()
        {
            AddValidationTerm( i => i !=null);
            
            AddValidationTerm( i => i.Request, new PactRequestValidator());
            
            AddValidationTerm( i => i.Response, new PactResponseValidator());
            
            AddValidationTerm( i => !string.IsNullOrEmpty(i.ProviderState));
        }
    }
}