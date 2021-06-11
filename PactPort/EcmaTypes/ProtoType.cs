using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.EcmaTypes
{
    public class ProtoType:Dictionary<string,EcmaType>
    {
        public bool Equals(ProtoType obj)
        {
            return Count == obj.Count && Covers(obj);
        }

        public override bool Equals(object obj)
        {
            if (obj is ProtoType protoType)
            {
                return this.Equals(protoType);
            }
            return false;
        }

        public bool Covers(ProtoType other)
        {
            foreach (var keyValuePair in other)
            {
                if (!ContainsKey(keyValuePair.Key))
                {
                    return false;
                }
                var myTypeForKey = this[keyValuePair.Key];
                
                if (!myTypeForKey.Equals(keyValuePair.Value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}