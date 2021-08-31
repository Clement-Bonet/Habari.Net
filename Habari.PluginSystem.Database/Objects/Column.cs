using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class Column
    {
        public string Name { get; set; }

        public DatabaseType Type { get; set; }

        public bool IsNullabled { get; set; }
    }
}
