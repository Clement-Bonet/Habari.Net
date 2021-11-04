using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habari.PluginSystem.Database.OData
{
    public enum Operator
    {
        [Description("and")]
        And,
        [Description("contains")]
        Contains,
        [Description("endswith")]
        EndsWith,
        [Description("eq")]
        Equal,
        [Description("gt")]
        GreaterThan,
        [Description("ge")]
        GreaterThanOrEqual,
        [Description("lt")]
        LessThan,
        [Description("le")]
        LessThanOrEqual,
        [Description("in")]
        In,
        [Description("not")]
        Not,
        [Description("ne")]
        NotEqual,
        [Description("or")]
        Or,
        [Description("startwith")]
        StartWith,
    }
}
