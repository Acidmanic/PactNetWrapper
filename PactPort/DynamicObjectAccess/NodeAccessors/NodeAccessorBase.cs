using System;
using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.NodeAccessors
{
    public abstract class NodeAccessorBase<TData> : INodeAccessor
    {
        protected Type _type = typeof(object);

        protected object _data = new object();

        protected KeyMaker _keyMaker = new KeyMaker();
        private INodeAccessor _nodeAccessorImplementation;


        public abstract object GetChild(string key);
        public abstract ICollection<string> GetChildren();
        public abstract bool Supports(Type type);
        public abstract bool Supports(object @object);

        public void Wrap(object data, Type type, KeyMaker keyMaker)
        {
            _data = data;

            _type = type;

            _keyMaker = keyMaker;

            OnWrap((TData) data);
        }

        protected bool Implements<TSuper>(Type implementer)
        {
            return Implements(typeof(TSuper), implementer);
        }

        protected bool Extends<TSuper>(Type derivative)
        {
            return Extends(typeof(TSuper), derivative);
        }

        protected bool IsDerivedFrom<TSuper>(Type derivative)
        {
            return IsDerivedFrom(typeof(TSuper), derivative);
        }

        protected bool Implements(Type super, Type derivative)
        {
            var interfaces = GetInterfaces(derivative);

            foreach (var i in interfaces)
            {
                if (i.FullName != null && i.FullName.Equals(super.FullName))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool Extends(Type super, Type derivative)
        {
            var ancestry = GetAncestry(derivative);

            foreach (var parent in ancestry)
            {
                if (parent.FullName != null && parent.FullName.Equals(super.FullName))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool IsDerivedFrom(Type super, Type derivative)
        {
            return Implements(super, derivative) || Extends(super, derivative);
        }

        private List<Type> GetAncestry(Type derivative)
        {
            var ans = new List<Type>();

            var parent = derivative;

            while (parent != null)
            {
                ans.Add(parent);

                parent = parent.BaseType;
            }

            return ans;
        }

        private List<Type> GetInterfaces(Type type)
        {
            var interfaces = new List<Type>();

            var parent = type;

            while (parent != null)
            {
                interfaces.AddRange(parent.GetInterfaces());

                parent = parent.BaseType;
            }

            return interfaces;
        }

        protected virtual void OnWrap(TData data)
        {
        }

        public virtual string SeparatorFromParent => ".";
    }
}