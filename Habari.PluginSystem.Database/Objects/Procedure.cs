using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class Procedure
    {
        public string Name { get; set; }

        public Dictionary<int, SourceCodeParameter> Parameters { get; } = new Dictionary<int, SourceCodeParameter>();
    }
}
