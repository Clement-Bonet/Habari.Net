using Habari.PluginSystem.Database.Objects;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;

namespace Habari.PluginSystem.Database
{
    internal class ElementPlugin : IPlugin
    {
        public IConfiguration Configuration { get; protected set; }

        public bool Configure(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            Configuration = new ElementConfiguration();
            Configuration.LoadFromJToken(token, defaultPageSize, allowDelete, allowGet, allowPost, allowPut);
            return true;
        }

        public bool MatchTypeAndName(string type, string name)
        {
            return ((ElementConfiguration)Configuration).MatchTypeAndName(type, name);
        }

        public bool Process(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        {
            statusCode = HttpStatusCode.OK;
            return true;
        }
    }
}
