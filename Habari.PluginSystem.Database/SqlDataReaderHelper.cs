using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Habari.PluginSystem.Database
{
    public static class SqlDataReaderHelper
    {
        private static IEnumerable<Dictionary<string, object>> Serialize(DbDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols, DbDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        public static T To<T>(this DbDataReader r) where T : new()
        {
            T result = new T();

            return result;
        }

        public static string ToJson(this DbDataReader r)
        {
            return JsonConvert.SerializeObject(Serialize(r), Formatting.Indented);
        }
    }
}
