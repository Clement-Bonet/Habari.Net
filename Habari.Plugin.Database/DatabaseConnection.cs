using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;

namespace Habari.Plugin.Database
{
    public class DatabaseConnection : Connection
    {
        protected internal class Element : Connection
        {
            protected int DefaultPageSize { get; set; }

            protected string ObjectName { get; set; }

            protected string ObjectType { get; set; }

            public override string Type { get { return "Element"; } }

            protected List<string> Keys { get; } = new List<string>();

            public void LoadFromJToken(JToken element, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
            {
                Keys.Clear();
                AllowDelete = element.ValueOrDefault("allowDelete", allowDelete);
                AllowGet = element.ValueOrDefault("allowSelect", allowGet);
                AllowPost = element.ValueOrDefault("allowInsert", allowPost);
                AllowPut = element.ValueOrDefault("allowUpdate", allowPut);
                DefaultPageSize = element.ValueOrDefault("defaultPageSize", defaultPageSize);
                Keys.AddRange(element["keys"].Children().Select(i => i.Value<string>()));
                MetadataUri = element.Value<string>("metadataUri");
                ObjectName = element.Value<string>("objectName");
                ObjectType = element.Value<string>("objectType");
                Uri = element.Value<string>("uri");
            }

            public override HttpStatusCode Process(HttpListenerContext context, ref object content)
            {
                return base.Process(context, ref content);
            }

            public HttpStatusCode ProcessDelete(HttpListenerContext context)
            {
                return HttpStatusCode.OK;
            }

            public HttpStatusCode ProcessGet(HttpListenerContext context)
            {
                return HttpStatusCode.OK;
            }

            public HttpStatusCode ProcessPost(HttpListenerContext context)
            {
                return HttpStatusCode.OK;
            }

            public HttpStatusCode ProcessPut(HttpListenerContext context)
            {
                return HttpStatusCode.OK;
            }
        }

        public override string Type { get { return "Database"; } }

        protected string DatabaseEngine { get; set; }

        protected int DefaultPageSize { get; set; }

        protected List<Element> Elements { get; set; } = new List<Element>();

        protected string Host { get; set; }

        protected string Login { get; set; }

        protected int MaxSimultaneousConnections { get; set; }

        protected string Password { get; set; }

        protected int Port { get; set; }

        protected string Protocol { get; set; }

        protected string ServiceName { get; set; }

        protected bool SupportTransaction { get; set; }

        public override void LoadFromJToken(JToken connection)
        {
            AllowDelete = connection.Value<bool>("allowDelete");
            AllowGet = connection.Value<bool>("allowSelect");
            AllowPost = connection.Value<bool>("allowInsert");
            AllowPut = connection.Value<bool>("allowUpdate");
            MetadataUri = connection.Value<string>("metadataUri");
            Uri = connection.Value<string>("uri");
            DatabaseEngine = connection.Value<string>("databaseEngine");
            DefaultPageSize = connection.Value<int>("defaultPageSize");
            Host = connection.Value<string>("host");
            Login = connection.Value<string>("login");
            MaxSimultaneousConnections = connection.Value<int>("maxSimultaneousConnections");
            Password = connection.Value<string>("password");
            Port = connection.Value<int>("port");
            Protocol = connection.ValueOrDefault("protocol", "tcp");
            ServiceName = connection.ValueOrDefault("serviceName", "Habari.Database." + connection.Value<string>("databaseEngine"));
            SupportTransaction = connection.Value<bool>("supportTransaction");
            foreach (JToken element in connection["elements"].Children())
            {
                Element elm = new Element();
                elm.LoadFromJToken(element, DefaultPageSize, AllowDelete, AllowGet, AllowPost, AllowPut);
                Elements.Add(elm);
            }
        }

        public override HttpStatusCode Process(HttpListenerContext context, ref object content)
        {
            return base.Process(context, ref content);
        }

        public HttpStatusCode ProcessDelete(HttpListenerContext context)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ProcessGet(HttpListenerContext context)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ProcessPost(HttpListenerContext context)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ProcessPut(HttpListenerContext context)
        {
            return HttpStatusCode.OK;
        }

        public DbConnection GetConnection()
        {
            DbConnection result = null;
            switch (DatabaseEngine.ToUpper())
            {
                case "ORACLE":
                    result = new OracleConnection();
                    result.ConnectionString = "user id=" + Login + ";password=" + Password + ";data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=" + Protocol + ")(HOST=" + Host + ")(PORT=" + Port + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")))";
                    result.Open();
                    break;
                default:
                    throw (new Exception("Unhandled database connection"));
            }
            return result;
        }
    }
}
