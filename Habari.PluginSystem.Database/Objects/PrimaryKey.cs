using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class PrimaryKey
    {
        public string Name { get; set; }

        public Dictionary<int, Column> Columns { get; } = new Dictionary<int, Column>();
    }
}
