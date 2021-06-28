using Pact.Provider.Wrapper.PactPort;
using Pact.Provider.Wrapper.PactPort.EcmaTypes;
// ReSharper disable once CheckNamespace
namespace System
{
    public static class ObjectExtensions
    {

        public static EcmaType GetEcmaType(this object obj)
        {
            if (obj == null)
            {
                return EcmaTypes.Undefined;
            }

            if (IsFloatingPoint(obj) || IsInteger(obj))
            {
                return EcmaTypes.Number;
            }

            if (obj is bool)
            {
                return EcmaTypes.Boolean;
            }

            if (obj is char || obj is string)
            {
                return EcmaTypes.String;
            }

            if (IsLongInteger(obj))
            {
                return EcmaTypes.BigInt;
            }

            return GetObjectEcmaType(obj);
        }

        private static EcmaType GetObjectEcmaType(object o)
        {
            var flattened = new DynamicObjectAccess().Flatten(o,"");
            
            var prototype = new ProtoType();
            
            foreach (var keyValuePair in flattened)
            {
                prototype.Add(keyValuePair.Key,keyValuePair.GetEcmaType());
            }

            var objectEcmaType = EcmaTypes.ObjectType(prototype);
            
            return objectEcmaType;
        }

        private static bool IsFloatingPoint(object value)
        {
            return value is float || value is double ||
                   value is decimal;
        }

        private static bool IsInteger(object value)
        {
            return value is int || value is short
                   || value is byte || value is uint
                   || value is ushort;
        }
        
        
        private static bool IsLongInteger(object value)
        {
            return  value is long || value is ulong;
        }
        
    }
    
    
    
}