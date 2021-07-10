using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.Plugin
{
    public static class JTokenExtension
    {
        public static T ValueOrDefault<T>(this JToken token, object key, T defaultValue)
        {
            T result;
            try
            {
                result = token.Value<T>(key);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
