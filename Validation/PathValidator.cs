using System;

namespace Pact.Provider.Wrapper.Validation
{
    public class PathValidator:ModelValidatorBase<string>
    {
        public PathValidator()
        {
            AddValidationTerm( p => !string.IsNullOrEmpty(p));
            
            AddValidationTerm(p => Uri.TryCreate(p,UriKind.RelativeOrAbsolute, out var unUsedResult));
        }
    }
}