using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Template
{
    public class TemplateConfiguration : IConfiguration
    {
        public bool AllowDelete => throw new NotImplementedException();

        public bool AllowGet => throw new NotImplementedException();

        public bool AllowPost => throw new NotImplementedException();

        public bool AllowPut => throw new NotImplementedException();

        public int DefaultPageSize => throw new NotImplementedException();

        public string MetadataUri => throw new NotImplementedException();

        public string Type => throw new NotImplementedException();

        public string RestUri => throw new NotImplementedException();

        public void LoadFromJToken(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            throw new NotImplementedException();
        }
    }
}
