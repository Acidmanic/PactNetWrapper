using System.Collections.Generic;
using System.Reflection;

namespace Pact.Provider.Wrapper.PactPort.DynamicObjectAccess.ChildEnumerators
{
    public class ObjectPropertyEnumerator:IChildEnumerator
    {
        public Dictionary<string, object> Enumerate(object data)
        {
            var type = data.GetType();
                    
            var properties = type.GetProperties();
            
            Dictionary<string, object>  result = new Dictionary<string, object>();
            
            foreach (var property in properties)
            {
                // POCO
                if (property.CanWrite && property.CanRead)
                {
                    try
                    {
                        var value = property.GetValue(data);

                        var key = property.Name;

                        result.Add(key,value);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return result;
        }

        public bool Supports(object data)
        {
            return data != null;
        }
    }
}