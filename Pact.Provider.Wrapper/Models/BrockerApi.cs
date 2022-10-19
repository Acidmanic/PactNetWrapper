using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Pact.Provider.Wrapper.Models
{
    public class BrockerApi : Dictionary<string, object>
    {
        public List<Link> Curies
        {
            get
            {
                if (this.ContainsKey("curies"))
                {
                    var curies = base["curies"];

                    if (curies is JArray array)
                    {
                        var links = new List<Link>();

                        for (int i = 0; i < array.Count; i++)
                        {
                            links.Add((Link)array[i].ToObject(typeof(Link)));
                        }

                        return links;
                    }

                    if (curies is List<Link> list)
                    {
                        return list;
                    }
                    
                }
                return new List<Link>();
            }
            set { base["curies"] = value; }
        }

        public new Link this[string key]
        {
            get
            {
                if (key != "curies")
                {
                    return (Link) base[key];
                }

                return null;
            }
            set
            {
                if (key != "curies")
                {
                    base[key] = value;
                }
            }
        }

        public BrockerApi Clone()
        {
            BrockerApi clone = new BrockerApi();

            foreach (var key in base.Keys)
            {
                var value = base[key];

                clone.Add(key, value);
            }

            return clone;
        }
    }
}