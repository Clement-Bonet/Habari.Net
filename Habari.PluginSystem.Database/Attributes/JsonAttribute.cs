using System;
using System.Collections.Generic;
using System.Text;

namespace Habari.PluginSystem.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonAttribute : Attribute
    {
        public string Name { get; private set; }

        public JsonAttribute(string name)
        {
            Name = name;
        }
    }
}
