using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class Package
    {
        public PackageBody Body { get; set; }

        public PackageHeader Header { get; set; }

        public string Name { get;  set; }
    }
}
