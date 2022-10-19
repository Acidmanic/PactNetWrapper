namespace Pact.Provider.Wrapper.PactPort.EcmaTypes
{
    public abstract class EcmaType
    {
        private readonly string _name;

        internal EcmaType(string name, int uniqueCompareCode)
        {
            _name = name;

            UniqueCompareCode = uniqueCompareCode;
        }

        public string Name => _name;

        private int UniqueCompareCode { get; }

        public override bool Equals(object obj)
        {
            if (obj is EcmaType ecmaObj)
            {
                return UniqueCompareCode == ecmaObj.UniqueCompareCode;
            }

            return false;
        }
        
        public abstract ProtoType ProtoType { get; }
    }
}