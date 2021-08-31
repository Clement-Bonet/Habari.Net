using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Habari.PluginSystem.Database
{
    public class OData
    {
        public List<string> Columns { get; } = new List<string>();

        public string Table { get; private set; }

        public List<string> Wheres { get; } = new List<string>();

        public List<string> OrderBys { get; } = new List<string>();

        public List<string> GroupBys { get; } = new List<string>();

        public int? Top { get; private set; }

        public int? Skip { get; private set; }

        public string ToSql(int defaultPageSize)
        {
            string result = "SELECT " + (Columns.Count > 0 ? string.Join(",", Columns) : "*") + " FROM " + Table + " WHERE 1=1 " + string.Join(" AND ", Wheres);

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

        public static OData Parse(string tableName, NameValueCollection value)
        {
            OData data = new OData();
            if (value.AllKeys.Any(k => k == "$Columns"))
            {
                data.Columns.AddRange(value["$Columns"].Split(",").ToList());
            }

            if (value.AllKeys.Any(k => k == "$Wheres"))
            {
                data.Wheres.AddRange(value["$Wheres"].Split(",").ToList());
            }

            return data;
        }

        public static OData Parse(string tableName, string value)
        {
            return JsonConvert.DeserializeObject<OData>(value);
        }

        public static bool TryParse(string tableName, NameValueCollection value, out OData data)
        {
            bool result = false;
            data = null;
            try
            {
                data = Parse(tableName, value);
                result = true;
            }
            catch { }

            return result;
        }

        public static bool TryParse(string tableName, string value, out OData data)
        {
            bool result = false;
            data = null;
            try
            {
                data = Parse(tableName, value);
                result = true;
            }
            catch { }

            return result;
        }
    }
}
