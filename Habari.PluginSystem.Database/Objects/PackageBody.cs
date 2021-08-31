using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Objects
{
    public class PackageBody
    {
        public List<Function> Functions { get; } = new List<Function>();

        public List<Procedure> Procedures { get; } = new List<Procedure>();
    }
}
