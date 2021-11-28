namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.Evaluators
{
    public class PrimitiveEvaluator:IEvaluator
    {
        public bool IsFlatEnough(object data)
        {
            return IsPrimitive(data);
        }

        public object Evaluate(object data)
        {
            return data;
        }

        public bool Supports(object data)
        {
            return true;
        }


        private bool IsPrimitive(object data)
        {
            if (data == null)
            {
                return false;
            }

            return data is string || data.GetType().IsPrimitive;
        }
    }
}