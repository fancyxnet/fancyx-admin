using System.ComponentModel;

namespace Fancyx.Core.Connection
{
    public enum DbType
    {
        [Description("PostgreSql")]
        PostgreSql = 1,

        [Description("MySql")]
        MySql = 2
    }
}