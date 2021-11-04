using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habari.PluginSystem.Database.OData
{
    public class Filter
    {
        public object FirstOperand { get; set; }

        public Operator Operator { get; }

        public object SecondOperand { get; set; }

    }
}
