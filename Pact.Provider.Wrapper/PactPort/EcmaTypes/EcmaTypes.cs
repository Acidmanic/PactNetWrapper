namespace Pact.Provider.Wrapper.PactPort.EcmaTypes
{
    public static class EcmaTypes
    {

        private class NumberEcmaType : EcmaType
        {
            internal NumberEcmaType() : base("Number",0)
            {
            }

            public override ProtoType ProtoType => new ProtoType();
        }
        public static EcmaType Number => new NumberEcmaType(); 
        
        private class StringEcmaType : EcmaType
        {
            internal StringEcmaType() : base("String",1)
            {
            }
            public override ProtoType ProtoType => new ProtoType();
        }
        public static EcmaType String => new StringEcmaType();
        
        private class BigIntEcmaType : EcmaType
        {
            internal BigIntEcmaType() : base("BigInt",2)
            {
            }
            public override ProtoType ProtoType => new ProtoType();
        }
        public static EcmaType BigInt => new BigIntEcmaType();
        
        private class BooleanEcmaType : EcmaType
        {
            internal BooleanEcmaType() : base("Boolean",3)
            {
            }
            public override ProtoType ProtoType => new ProtoType();
        }
        public static EcmaType Boolean => new BooleanEcmaType();
        
        private class ObjectEcmaType : EcmaType
        {
            internal ObjectEcmaType() : this(new ProtoType()) { }
            internal ObjectEcmaType(ProtoType protoType) : base("Object",4)
            {
                ProtoType = protoType;
            }
            public override ProtoType ProtoType { get; }
        }
        public static EcmaType Object => new ObjectEcmaType();
        
        private class UndefinedEcmaType : EcmaType
        {
            internal UndefinedEcmaType() : base("Undefined",5)
            {
            }
            public override ProtoType ProtoType => new ProtoType();
        }
        public static EcmaType Undefined => new UndefinedEcmaType();

        public static EcmaType ObjectType(ProtoType protoType)
        {
            return new ObjectEcmaType(protoType);
        }
        
    }
}