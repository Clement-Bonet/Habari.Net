using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class Table
    {
        public Dictionary<int, Column> Columns { get; } = new Dictionary<int, Column>();

        public List<ForeignKey> ForeignKeys { get; } = new List<ForeignKey>();

        public List<Index> Indexes { get; } = new List<Index>();

        public string Name { get; set; }

        public PrimaryKey PrimaryKey { get; set; }
    }
}
