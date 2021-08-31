using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem
{
    public static class JTokenExtension
    {
        public static TEnum ValueFromEnum<TEnum>(this JToken token, object key) where TEnum : struct
        {
            return Enum.Parse<TEnum>(token.Value<string>(key), true);
        }

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
