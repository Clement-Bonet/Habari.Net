using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Habari.PluginSystem.Database
{
    public static class SqlDataReaderHelper
    {
        public static T To<T>(this DbDataReader r) where T : new()
        {
            T result = new T();

            return result;
        }
    }
}
