using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.Validation
{
    public class PactResponseValidator:ModelValidatorBase<PactResponse>
    {
        public PactResponseValidator()
        {
            AddValidationTerm(r => r != null);
            
            AddValidationTerm(r => r.Status,new StatusCodeValidator());
        }
    }
}