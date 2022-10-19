using System;
using System.Collections;
using Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.NodeAccessors;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public class NodeAccessorFactory
    {
        private static readonly Func<INodeAccessor>[] Accessors =
        {
            () => new JObjectNodeAccessor(),
            () => new HashTableNodeAccessor(),
            () => new EnumerableNodeAccessor(),
            () => new ObjectPropertyNodeAccessor(),
        };

        public INodeAccessor Make(object data)
        {
            for (var i = 0; i < Accessors.Length; i++)
            {
                var ax = Accessors[i]();

                if (ax.Supports(data))
                {
                    return ax;
                }
            }

            return new NullAccessor();
        }

        public INodeAccessor Make(Type type)
        {
            for (var i = 0; i < Accessors.Length; i++)
            {
                var ax = Accessors[i]();

                if (ax.Supports(type))
                {
                    return ax;
                }
            }

            return new NullAccessor();
        }
    }
}