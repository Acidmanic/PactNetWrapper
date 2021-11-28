using System.Collections;
using Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public class ChildEnumeratorFactory
    {

        private static readonly IChildEnumerator[] Enumerators =
        {
            new JObjectChildEnumerator(),
            new HashtableChildEnumerator(), 
            new EnumerableChildEnumerator(),
            new ObjectPropertyEnumerator(), 
        };
        
        public IChildEnumerator MakeEnumerator(object data)
        {

            for (var i = 0; i < Enumerators.Length; i++)
            {
                if (Enumerators[i].Supports(data))
                {
                    return Enumerators[i];
                }
            }
            
            return new NullEnumerator();
        }
    }
}