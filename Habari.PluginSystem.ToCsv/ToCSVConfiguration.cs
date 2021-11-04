using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Template
{
    public class ToCSVConfiguration : IConfiguration
    {
        public bool AllowDelete => false;

        public bool AllowGet => true;

        public bool AllowPost => false;

        public bool AllowPut => false;

        public string MetadataUri { get; set; }

        public string Type => "ToCsv";

        public string RestUri { get; set; }

        public void LoadFromJToken(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            MetadataUri = token.Value<string>("metadataUri");
            RestUri = token.Value<string>("restUri");
        }
    }
}
