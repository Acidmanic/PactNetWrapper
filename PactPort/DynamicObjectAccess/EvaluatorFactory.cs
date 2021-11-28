using System.Collections;
using Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators;
using Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.Evaluators;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public class EvaluatorFactory
    {

        private static readonly IEvaluator[] Enumerators =
        {
            new JValueEvaluator(),
            new PrimitiveEvaluator(), 
        };
        
        public IEvaluator MakeEnumerator(object data)
        {

            for (var i = 0; i < Enumerators.Length; i++)
            {
                if (Enumerators[i].Supports(data))
                {
                    return Enumerators[i];
                }
            }
            
            return new NullEvaluator();
        }
    }
}