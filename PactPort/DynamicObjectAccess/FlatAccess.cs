using System;
using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess
{
    public class FlatAccess
    {
        public KeyMaker KeyMaker { get; set; }

        private readonly NodeAccessorFactory _accessorFactory;
        private readonly EvaluatorFactory _evaluatorFactory;

        public FlatAccess() : this(false)
        {
        }

        public FlatAccess(bool camelCase)
        {
            KeyMaker = new KeyMaker(camelCase);

            _accessorFactory = new NodeAccessorFactory();

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
            if (data is null)
            {
                return;
            }
            
            var evaluator = _evaluatorFactory.Make(data);

            if (evaluator.IsFlatEnough(data))
            {
                var value = evaluator.Evaluate(data);

                result.Add(prefix, value);

                return;
            }

            var accessor = _accessorFactory.Make(data);

            accessor.Wrap(data, data.GetType(), KeyMaker);

            var rawChildren = accessor.GetChildren();

            foreach (var child in rawChildren)
            {
                string key = KeyMaker.MakePath(prefix, child, accessor.SeparatorFromParent);

                var value = accessor.GetChild(child);

                FlattenRecursive(value, key, result);
            }
        }

        // public void LoadInto(object @object, Dictionary<string, object> data)
        // {
        //     LoadInto(@object, data, "");
        // }
        //
        // private void LoadInto(object @object, Dictionary<string, object> data, string prefix)
        // {
        //     if (data != null)
        //     {
        //         var properties = @object.GetType().GetProperties();
        //
        //         foreach (var property in properties)
        //         {
        //             var type = property.PropertyType;
        //
        //             if (type.IsPrimitive)
        //             {
        //                 if (property.CanWrite && data.ContainsKey(prefix))
        //                 {
        //                     var value = data[prefix];
        //
        //                     property.SetValue(@object, value);
        //                 }
        //             }
        //             else
        //             {
        //                 if (property.CanRead)
        //                 {
        //                     var childObject = property.GetValue(@object);
        //
        //                     var name = SetCase(property.Name);
        //
        //                     var key = InnerKey(prefix, name, ".");
        //
        //                     LoadInto(childObject, data, key);
        //
        //                     if (property.CanWrite)
        //                     {
        //                         property.SetValue(@object, childObject);
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }
        //
        // public Hashtable LoadInto(Hashtable @object, Dictionary<string, object> data, string prefix = "")
        // {
        //     Hashtable result = new Hashtable();
        //
        //     if (@object != null)
        //     {
        //         foreach (DictionaryEntry entry in @object)
        //         {
        //             object child;
        //
        //             var key = (prefix.Length > 0 ? prefix + "." : "") + entry.Key;
        //
        //             if (entry.Value is Hashtable childObject)
        //             {
        //                 child = LoadInto(childObject, data, key);
        //             }
        //             else
        //             {
        //                 if (data.ContainsKey(key))
        //                 {
        //                     child = data[key];
        //                 }
        //                 else
        //                 {
        //                     child = default;
        //                 }
        //             }
        //
        //             result.Add(entry.Key, child);
        //         }
        //     }
        //
        //     return result;
        // }
    }
}