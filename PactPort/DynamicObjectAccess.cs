using System.Collections;
using System.Collections.Generic;

namespace Pact.Provider.Wrapper.PactPort
{
    public class DynamicObjectAccess
    {
        private readonly bool _camelCase = false;


        public DynamicObjectAccess()
        {
            _camelCase = false;
        }

        public DynamicObjectAccess(bool camelCase)
        {
            _camelCase = camelCase;
        }

        public Dictionary<string, object> Flatten(object data, string prefix)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            Flatten(data, prefix, result);

            return result;
        }
        
        public void Flatten(object data, string prefix, Dictionary<string, object> result)
        {
            if (data != null)
            {
                var type = data.GetType();

                if (type.IsPrimitive)
                {
                    result.Add(prefix, data);
                }
                else
                {
                    var properties = type.GetProperties();

                    foreach (var property in properties)
                    {
                        if (property.CanWrite && property.CanRead)
                        {
                            try
                            {
                                var value = property.GetValue(data);

                                var key = SetCase(property.Name);

                                Flatten(value, key, result);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                }
            }
        }

        public void LoadInto(object @object, Dictionary<string, object> data)
        {
            LoadInto(@object,data,"");
        }
        
        public void LoadInto(object @object, Dictionary<string, object> data,string prefix)
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
                            
                            property.SetValue(@object,value);
                        }
                    }
                    else
                    {
                        if (property.CanRead)
                        {
                            var childObject = property.GetValue(@object);

                            var name = SetCase(property.Name);

                            var key = (prefix.Length > 0 ? prefix + "." : "") + name;
                            
                            LoadInto(childObject,data,key);

                            if (property.CanWrite)
                            {
                                property.SetValue(@object,childObject);
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

            if (_camelCase)
            {
                return char.ToUpper(firstChar) + rest;
            }
            else
            {
                return char.ToLower(firstChar) + rest;
            }
        }
        
        public Dictionary<string, object> Flatten(Hashtable data, string prefix)
        {
            var result = new Dictionary<string, object>();

            Flatten(prefix, data, result);

            return result;
        }

        private void Flatten(string prefix, Hashtable data, Dictionary<string, object> result)
        {
            if (data != null)
            {
                foreach (DictionaryEntry entry in data)
                {
                    if (entry.Value is Hashtable)
                    {
                        Flatten(prefix + "." + entry.Key, entry.Value as Hashtable, result);
                    }
                    else
                    {
                        result.Add(prefix + "." + (string) entry.Key, entry.Value);
                    }
                }
            }
        }
        
        private void LoadInto(Hashtable @object, Dictionary<string, object> data,string prefix)
        {
            if (@object != null)
            {
                foreach (DictionaryEntry entry in @object)
                {
                    object child;
                    
                    var key = (prefix.Length > 0 ? prefix + "." : "") + entry.Key;
                    
                    if (entry.Value is Hashtable childObject)
                    {
                        LoadInto(childObject,data,key);

                        child = childObject;
                    }
                    else
                    {
                        if (data.ContainsKey(key))
                        {
                            child = data[prefix];
                        }
                        else
                        {
                            child = default;
                        }
                        @object.Remove(entry.Key);
                        
                        @object.Add(entry.Key,child);
                    }
                }
            }
        }
    }
}