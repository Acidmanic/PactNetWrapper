namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public interface IEvaluator
    {
        bool IsFlatEnough(object data);

        object Evaluate(object data);

        bool Supports(object data);
    }
}