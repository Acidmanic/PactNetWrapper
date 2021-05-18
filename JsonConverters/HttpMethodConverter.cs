using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Pact.Provider.Wrapper.JsonConverters
{
    public class HttpMethodConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            HttpMethod method = (HttpMethod) value;

            writer.WriteValue(method.Method.ToUpper());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            String value = (string) reader.Value;

            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToUpper();

                var methods = new HttpMethod[]
                {
                    HttpMethod.Get, HttpMethod.Delete,
                    HttpMethod.Head, HttpMethod.Options,
                    HttpMethod.Post, HttpMethod.Put, HttpMethod.Trace
                };

                foreach (var method in methods)
                {
                    if (value.Equals(method.Method.ToUpper()))
                    {
                        return method;
                    }
                }
            }

            return HttpMethod.Get;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(HttpMethod));
        }
    }
}