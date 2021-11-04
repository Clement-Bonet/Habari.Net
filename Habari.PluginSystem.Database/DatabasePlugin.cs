using Habari.PluginSystem.Database.Objects;
using Newtonsoft.Json.Linq;
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

        public bool Process(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        {
            OData.OData data;
            string tableName = context.Request.Url.AbsolutePath.Split('/').Last();
            OData.OData.TryParse(Configuration.RestUri, context.Request, out data);
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "DELETE":
                    statusCode = ProcessDelete(context, ref contentType, ref content);
                    break;
                case "GET":
                    statusCode = ProcessGet(context, data, tableName, ref contentType, ref content);
                    break;
                case "POST":
                    statusCode = ProcessPost(context, ref contentType, ref content);
                    break;
                case "PUT":
                    statusCode = ProcessPut(context, ref contentType, ref content);
                    break;
                default:
                    statusCode = HttpStatusCode.NotFound;
                    break;
            }
            return true;
        }

        public HttpStatusCode ProcessDelete(HttpListenerContext context, ref string contentType, ref object content)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ProcessGet(HttpListenerContext context, OData.OData oData, string tableName, ref string contentType, ref object content)
        {
            DbConnection cnn = ((DatabaseConfiguration)Configuration).GetDbConnection();
            DbCommand cmd = cnn.CreateCommand();
            string sql = "SELECT ";

            if (oData.Select.Any())
            {
                sql += string.Join(", ", oData.Select);
            }
            else
            {
                sql += "*";
            }

            sql += " FROM " + tableName;
            Dictionary<int, Column> columns = new Dictionary<int, Column>();
            if (Schema.Tables.Any(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)))
            {
                columns = Schema.Tables.First(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).Columns;
            }
            else if (Schema.Views.Any(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)))
            {
                columns = Schema.Views.First(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).Columns;
            }

            if (columns.Any())
            {
                sql += " WHERE 1=1";
                foreach (var item in context.Request.QueryString.AllKeys.Where(k => columns.Any(c => c.Value.Name.Equals(k, StringComparison.InvariantCultureIgnoreCase))))
                {
                    Column column = columns.First(c => c.Value.Name.Equals(item, StringComparison.InvariantCultureIgnoreCase)).Value;
                    sql += " AND " + item + " = " + (column.Type == DatabaseType.String ? "'" : string.Empty) + context.Request.QueryString[item] + (column.Type == DatabaseType.String ? "'" : string.Empty);
                }
            }

            cmd.CommandText = sql;
            DbDataReader reader = cmd.ExecuteReader();
            contentType = "application/json";
            content = reader.ToJson();
            return HttpStatusCode.OK;
        }

        //public bool ProcessMetadata(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        //{
        //    statusCode = HttpStatusCode.OK;
        //    contentType = "application/json";
        //    content = JsonConvert.SerializeObject(Schema);
        //    return true;
        //}

        public HttpStatusCode ProcessPost(HttpListenerContext context, ref string contentType, ref object content)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ProcessPut(HttpListenerContext context, ref string contentType, ref object content)
        {
            return HttpStatusCode.OK;
        }
    }
}
