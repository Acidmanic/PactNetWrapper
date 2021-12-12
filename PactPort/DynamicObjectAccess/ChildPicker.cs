using System;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public abstract class ChildPicker
    {
        protected readonly Func<string, string, string> KeyMaker;

        protected ChildPicker(Func<string, string, string, string> keyMaker)
        {
            KeyMaker = (p,k) => keyMaker(p,k,this.Separator);
        }


        public abstract ICollection<string> PickChildKeys(Type targetType, ICollection<string> keys, string prefix);

        public abstract bool Supports(object data);

        public virtual string Separator => ".";

        public abstract void SetInto(object parent, object property, string key);
    }
}