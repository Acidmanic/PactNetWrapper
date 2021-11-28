using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public class FlatAccess
    {
        
        public string Separator { get; set; }
        public bool CamelCase { get; set; }

        private readonly ChildEnumeratorFactory _enumeratorFactory;
        private readonly EvaluatorFactory _evaluatorFactory;
        
        public FlatAccess():this(false)
        {
            
        }
        public FlatAccess(bool camelCase):this(camelCase,".")
        {
        }
        public FlatAccess(bool camelCase,string separator)
        {
            CamelCase = camelCase;
            Separator = separator;
            _enumeratorFactory = new ChildEnumeratorFactory();
            _evaluatorFactory = new EvaluatorFactory();
        }

        public Dictionary<string, object> Flatten(object data, string prefix)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            FlattenRecursive(data, prefix, result);

            return result;
        }


        private void FlattenRecursive(object data, string prefix, Dictionary<string, object> result)
        {
            var evaluator = _evaluatorFactory.MakeEnumerator(data);

            if (evaluator.IsFlatEnough(data))
            {
                var value = evaluator.Evaluate(data);
                
                result.Add(prefix,value);
                
                return;
            }

            Dictionary<string, object> rawChildren = EnumerateChildren(data);

            foreach (var child in rawChildren)
            {
                string key = InnerKey(prefix, child.Key);
                
                FlattenRecursive(child.Value,key,result);
            }
        }

        private Dictionary<string, object> EnumerateChildren(object data)
        {
            var enumerator = _enumeratorFactory.MakeEnumerator(data);

            var children = enumerator.Enumerate(data);

            return children;
        }

        private string InnerKey(string prefix, string key)
        {
            return prefix + Separator + SetCase(key);
        }

        public void LoadInto(object @object, Dictionary<string, object> data)
        {
            LoadInto(@object, data, "");
        }

        private void LoadInto(object @object, Dictionary<string, object> data, string prefix)
        {
            if (data != null)
            {
                var properties = data.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var type = property.PropertyType;

                    if (type.IsPrimitive)
                    {
                        if (property.CanWrite && data.ContainsKey(prefix))
                        {
                            var value = data[prefix];

                            property.SetValue(@object, value);
                        }
                    }
                    else
                    {
                        if (property.CanRead)
                        {
                            var childObject = property.GetValue(@object);

                            var name = SetCase(property.Name);

                            var key = (prefix.Length > 0 ? prefix + "." : "") + name;

                            LoadInto(childObject, data, key);

                            if (property.CanWrite)
                            {
                                property.SetValue(@object, childObject);
                            }
                        }
                    }
                }
            }
        }

        private string SetCase(string id)
        {
            if (string.IsNullOrEmpty(id) || id.Length == 1)
            {
                return id;
            }

            char firstChar = id.ToCharArray()[0];

            string rest = id.Substring(1, id.Length - 1);

            if (CamelCase)
            {
                return char.ToLower(firstChar) + rest;
            }
            else
            {
                return char.ToUpper(firstChar) + rest;
            }
        }

        public Hashtable LoadInto(Hashtable @object, Dictionary<string, object> data, string prefix = "")
        {
            Hashtable result = new Hashtable();

            if (@object != null)
            {
                foreach (DictionaryEntry entry in @object)
                {
                    object child;

                    var key = (prefix.Length > 0 ? prefix + "." : "") + entry.Key;

                    if (entry.Value is Hashtable childObject)
                    {
                        child = LoadInto(childObject, data, key);
                    }
                    else
                    {
                        if (data.ContainsKey(key))
                        {
                            child = data[key];
                        }
                        else
                        {
                            child = default;
                        }
                    }

                    result.Add(entry.Key, child);
                }
            }

            return result;
        }
    }
}