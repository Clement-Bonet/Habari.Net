using Habari.PluginSystem.Database.Objects;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;

namespace Habari.PluginSystem.Database
{
    public class DatabasePlugin : IPlugin
    {
        public Schema Schema { get; protected set; }

        public IConfiguration Configuration { get; protected set; }

        public bool Configure(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            Configuration = new DatabaseConfiguration();
            Configuration.LoadFromJToken(token, defaultPageSize, allowDelete, allowGet, allowPost, allowPut);
            Schema = ((DatabaseConfiguration)Configuration).ExploreSchema();
            return true;
        }

        public bool Process(HttpListenerContext context, out HttpStatusCode statusCode, ref object content)
        {
            OData data;
            string tableName = context.Request.RawUrl.Split('/').Last();
            OData.TryParse(tableName, context.Request.QueryString, out data);
            statusCode = HttpStatusCode.OK;
            return true;
        }

        public HttpStatusCode ProcessDelete(HttpListenerContext context, ref object content)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ProcessGet(HttpListenerContext context, ref object content)
        {
            return HttpStatusCode.OK;
        }

        public bool ProcessMetadata(HttpListenerContext context, out HttpStatusCode statusCode, ref object content)
        {
            statusCode = HttpStatusCode.OK;
            return true;
        }

        public HttpStatusCode ProcessPost(HttpListenerContext context, ref object content)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ProcessPut(HttpListenerContext context, ref object content)
        {
            return HttpStatusCode.OK;
        }
    }
}
