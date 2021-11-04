using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

namespace Habari.PluginSystem.Template
{
    public class ToCSVPlugin : IPlugin
    {
        public IConfiguration Configuration { get; protected set; }

        public bool Configure(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            Configuration = new ToCSVConfiguration();
            Configuration.LoadFromJToken(token, defaultPageSize, allowDelete, allowGet, allowPost, allowPut);
            return true;
        }

        public bool Process(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        {
            switch (contentType.ToLower())
            {
                case "application/json":
                    StringWriter swJson = new StringWriter();
                    DataTable dtJson = JsonConvert.DeserializeObject<DataTable>((string)content);
                    using (var csv = new CsvWriter(swJson, CultureInfo.InvariantCulture))
                    {
                        foreach (DataColumn column in dtJson.Columns)
                        {
                            csv.WriteField(column.ColumnName);
                        }
                        csv.NextRecord();

                        foreach (DataRow row in dtJson.Rows)
                        {
                            for (var i = 0; i < dtJson.Columns.Count; i++)
                            {
                                csv.WriteField(row[i]);
                            }
                            csv.NextRecord();
                        }
                    }
                    contentType = "text/csv";
                    content = swJson.ToString();
                    statusCode = HttpStatusCode.OK;
                    break;
                case "application/xml":
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml((string)content);
                    StringWriter swXml = new StringWriter();
                    DataTable dtXml = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeXmlNode(xml));
                    using (var csv = new CsvWriter(swXml, CultureInfo.InvariantCulture))
                    {
                        foreach (DataColumn column in dtXml.Columns)
                        {
                            csv.WriteField(column.ColumnName);
                        }
                        csv.NextRecord();

                        foreach (DataRow row in dtXml.Rows)
                        {
                            for (var i = 0; i < dtXml.Columns.Count; i++)
                            {
                                csv.WriteField(row[i]);
                            }
                            csv.NextRecord();
                        }
                    }
                    contentType = "text/csv";
                    content = swXml.ToString();
                    statusCode = HttpStatusCode.OK;
                    break;
                default:
                    statusCode = HttpStatusCode.NotFound;
                    break;
            }
            
            return true;
        }

        //public bool ProcessMetadata(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
