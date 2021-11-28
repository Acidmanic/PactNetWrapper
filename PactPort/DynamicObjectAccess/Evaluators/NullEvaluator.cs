namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.Evaluators
{
    public class NullEvaluator:IEvaluator
    {
        public bool IsFlatEnough(object data)
        {
            return true;
        }

        public object Evaluate(object data)
        {
            return null;
        }

        public bool Supports(object data)
        {
            return false;
        }
    }
}