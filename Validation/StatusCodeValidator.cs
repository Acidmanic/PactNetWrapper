namespace Pact.Provider.Wrapper.Validation
{
    public class StatusCodeValidator:ModelValidatorBase<int>
    {
        public StatusCodeValidator()
        {
            AddValidationTerm(code => code > 99 && code <600 );
        }
    }
}