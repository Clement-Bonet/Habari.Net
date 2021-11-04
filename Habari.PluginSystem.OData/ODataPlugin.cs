using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace Habari.PluginSystem.OData
{
    public class ODataPlugin : IPlugin
    {
        public IConfiguration Configuration => throw new NotImplementedException();

        public bool Configure(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            throw new NotImplementedException();
        }

        public bool Process(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        {
            throw new NotImplementedException();
        }

        //public bool ProcessMetadata(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
