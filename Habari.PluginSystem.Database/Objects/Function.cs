using System.Collections.Generic;
using System.Linq;

namespace Habari.PluginSystem.Database.Objects
{
    public class Function
    {
        public Dictionary<int, SourceCodeParameter> Parameters { get; } = new Dictionary<int, SourceCodeParameter>();

        public string Name { get; set; }

        public DatabaseType ReturnType { get; set; }
    }
}
