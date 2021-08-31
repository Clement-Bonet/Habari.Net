using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class SourceCodeParameter
    {
        public SourceCodeParameterDirection Direction { get; set; }

        public string Name { get; set; }

        public DatabaseType Type { get; set; }
    }
}
