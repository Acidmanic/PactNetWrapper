using Newtonsoft.Json.Linq;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.Evaluators
{
    public class JValueEvaluator : IEvaluator
    {
        public bool IsFlatEnough(object data)
        {
            return data is JValue;
        }

        public object Evaluate(object data)
        {
            if (data is JValue jData)
            {
                return jData.Value;
            }

            return null;
        }

        public bool Supports(object data)
        {
            return data is JValue;
        }
    }
}