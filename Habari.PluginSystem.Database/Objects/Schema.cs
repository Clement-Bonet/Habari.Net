using System.Collections.Generic;

namespace Habari.PluginSystem.Database.Objects
{
    public class Schema
    {
        public string Name { get; set; }

        public List<Function> Functions { get; } = new List<Function>();

        public List<Package> Packages { get; } = new List<Package>();

        public List<Procedure> Procedures { get; } = new List<Procedure>();

        public List<Table> Tables { get; } = new List<Table>();

        public List<View> Views { get; } = new List<View>();
    }
}
