using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class ForeignKey
    {
        public string Name { get; set; }

        public Dictionary<int, Column> Columns { get; } = new Dictionary<int, Column>();

        public Table ReferencedTable { get; set; }
    }
}
