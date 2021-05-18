namespace Pact.Provider.Wrapper.Validation
{
    public class PactModelValidator:ModelValidatorBase<Models.Pact>
    {
        public PactModelValidator()
        {
            AddValidationTerm( p => p.Consumer?.Name!=null);
            
            AddValidationTerm( p => p.Provider?.Name!=null);
            
            AddValidationTerm( p => p.Interactions!=null);
            
            AddValidationTerm( p => p.Interactions.Count >0);
            
            AddValidationTerm( p => p.Interactions,new InteractionValidator());
        }
    }
}