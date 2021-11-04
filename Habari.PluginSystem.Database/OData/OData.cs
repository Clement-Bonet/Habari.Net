using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Habari.PluginSystem.Database.OData
{
    public class OData
    {
        public List<string> Select { get; } = new List<string>();
        
        public string Table { get; private set; }

        public List<Filter> Filters { get; } = new List<Filter>();

        public List<string> OrderBys { get; } = new List<string>();

        public List<string> GroupBys { get; } = new List<string>();

        public List<OData> Expands { get; } = new List<OData>();

        public int? Top { get; private set; }

        public int? Skip { get; private set; }

        public string ToSql(int defaultPageSize)
        {
            string result = "SELECT " + (Select.Count > 0 ? string.Join(",", Select) : "*") + " FROM " + Table + " WHERE 1=1 " + string.Join(" AND ", Filters);

            if (GroupBys.Count > 0)
            {
                result += " GROUP BY " + string.Join(",", GroupBys);
            }

            if (OrderBys.Count > 0)
            {
                result += " ORDER BY " + string.Join(",", OrderBys);
            }

            if (Top.HasValue || Skip.HasValue)
            {
                result += " OFFSET " + (Top ?? 0).ToString() + " FETCH NEW " + (Skip ?? defaultPageSize) + " ROWS ONLY";
            }

            return result;
        }

        public static OData Parse(string rootUri, HttpListenerRequest request)
        {
            OData data = new OData();
            string tableName = request.RawUrl.Replace(rootUri, string.Empty, StringComparison.InvariantCultureIgnoreCase);
            NameValueCollection value = request.QueryString;
            /*
            if (value.AllKeys.Any(k => k == "$select"))
            {
                data.Select.AddRange(value["$select"].Split(",").ToList().Where(i => !string.IsNullOrWhiteSpace(i)));
            }

            if (value.AllKeys.Any(k => k == "filter"))
            {
                data.Filters.AddRange(value["filter"].Split(",").ToList().Where(i => !string.IsNullOrWhiteSpace(i)));
            }
            */

            return data;
        }

        public static bool TryParse(string rootUri, HttpListenerRequest request, out OData data)
        {
            bool result = false;
            data = null;
            try
            {
                data = Parse(rootUri, request);
                result = true;
            }
            catch { }

            return result;
        }
    }
}
