using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Habari.PluginSystem.Database
{
    internal class ElementConfiguration : IConfiguration
    {
        public bool AllowDelete { get; protected set; }

        public bool AllowGet { get; protected set; }

        public bool AllowPost { get; protected set; }

        public bool AllowPut { get; protected set; }

        public int DefaultPageSize { get; protected set; }

        public string MetadataUri { get; protected set; }

        public string Type => "Element";

        public string RestUri { get; protected set; }

        protected string ObjectName { get; set; }

        protected string ObjectType { get; set; }

        public string ODataUri { get; private set; }

        public void LoadFromJToken(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            AllowDelete = token.ValueOrDefault("allowDelete", allowDelete);
            AllowGet = token.ValueOrDefault("allowSelect", allowGet);
            AllowPost = token.ValueOrDefault("allowInsert", allowPost);
            AllowPut = token.ValueOrDefault("allowUpdate", allowPut);
            DefaultPageSize = token.ValueOrDefault("defaultPageSize", defaultPageSize);
            MetadataUri = token.Value<string>("metadataUri");
            RestUri = token.Value<string>("restUri");
            ObjectName = token.Value<string>("objectName");
            ObjectType = token.Value<string>("objectType");
            ODataUri = token.Value<string>("odataUri");
        }
    }
}
