using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Template
{
    public class TemplateConfiguration : IConfiguration
    {
        public bool AllowDelete => false;

        public bool AllowGet => true;

        public bool AllowPost => true;

        public bool AllowPut => true;

        public string MetadataUri { get; protected set; }

        public string Type => "ToCSV";

        public string RestUri { get; protected set; }

        public void LoadFromJToken(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            MetadataUri = token.Value<string>("metadataUri");
            RestUri = token.Value<string>("restUri");
        }
    }
}
