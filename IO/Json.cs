using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pact.Provider.Wrapper.JsonConverters;
using Pact.Provider.Wrapper.Validation;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.Wrapper.IO
{
    public class Json
    {
        public string ToString(Models.Pact pact)
        {
            var settings = CreateSettings();

            string json = JsonConvert.SerializeObject(pact, settings);

            return json;
        }


        public Models.Pact ToPact(string json)
        {
            var settings = CreateSettings();

            var pact = JsonConvert.DeserializeObject<Models.Pact>(json, settings);

            return pact;
        }

        public void Save(string file, Models.Pact pact)
        {
            string content = ToString(pact);

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            File.WriteAllText(file, content);
        }

        public Models.Pact Load(string path)
        {
            string content = File.ReadAllText(path);
            
            var pact = ToPact(content);

            return pact;
        }

        private JsonSerializerSettings CreateSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                CheckAdditionalContent = false
            };
            settings.Converters.Add(new HttpMethodConverter());

            return settings;
        }


        public List<Models.Pact> LoadDirectory(string path)
        {
            var files = Directory.EnumerateFiles(path);
            
            var pacts = new List<Models.Pact>();
            
            foreach (var file in files)
            {
                try
                {
                    if (file.ToLower().EndsWith(".json"))
                    {
                        var pact = new Json().Load(file);

                        if (new PactModelValidator().Validate(pact))
                        {
                            pacts.Add(pact);
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return pacts;
        }
    }
}