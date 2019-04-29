using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio
{
    public static class JsonUtil
    {
        public static JToken GetOrCreateProperty(this JToken token, string key)
        {
            var t = token[key];
            if (t == null)
            {
                t = new JObject();
                token[key] = t;
            }
            return t;
        }
    }
}
