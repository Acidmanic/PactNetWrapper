using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.Validation
{
    public class PactRequestValidator:ModelValidatorBase<PactRequest>
    {
        public PactRequestValidator()
        {
            AddValidationTerm(r => r != null);
            
            AddValidationTerm(( r => !string.IsNullOrEmpty(r.Path)));
            
            AddValidationTerm(r => r.Path, new PathValidator());
        }
    }
}