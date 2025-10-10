using DatabaseType = Fancyx.Core.Connection.DbType;

namespace Fancyx.Core.Connection
{
    public class ConnectionStringOption
    {
        public string DbType { get; set; } = DatabaseType.PostgreSql.ToString();
        public string? PostgreSql { get; set; }
        public string? MySql { get; set; }

        public DatabaseType DatabaseType => Enum.Parse<DatabaseType>(DbType);

        public string GetConnectionString()
        {
            return DatabaseType switch
            {
                DatabaseType.PostgreSql => PostgreSql ?? throw new ArgumentNullException("PostgreSql", "PostgreSql连接字符串不能为空"),
                DatabaseType.MySql => MySql ?? throw new ArgumentNullException("MySql", "MySql连接字符串不能为空"),
                _ => throw new NotSupportedException($"不支持的数据库类型 => {DbType}"),
            };
        }
    }
}