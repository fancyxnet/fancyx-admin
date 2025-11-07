using Dapper;
using Fancyx.Admin.EfCore.Models;
using Fancyx.Core.AutoInject;
using Fancyx.EfCore;
using Fancyx.EfCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Fancyx.Admin.EfCore.Repositories
{
    [DenpendencyInject(AsSelf = true)]
    public class GenRepository
    {
        private readonly DbContext _context;

        public GenRepository(DbContext dbContext)
        {
            _context = dbContext;
        }

        public IDbConnection Connection => _context.Database.GetDbConnection();

        //public string GetDatabase()
        //{
        //    var connectionString = _context.Database.GetConnectionString();
        //    var str = connectionString!.Split(';').FirstOrDefault(x => x.Contains("database=") || x.Contains("Database="));
        //    return str!.Replace("database=", "").Replace("Database=", "");
        //}

        /// <summary>
        /// 列信息查询
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public async Task<List<ColumnInfo>> QueryColumnsAsync(string table)
        {
            var sql = @"SELECT 
                            COLUMN_NAME AS ColumnName,
                            COLUMN_TYPE AS ColumnType,
                            IS_NULLABLE AS IsNullable,
                            COLUMN_DEFAULT AS ColumnDefault,
                            COLUMN_KEY AS ColumnKey,
                            EXTRA AS Extra,
                            COLUMN_COMMENT AS ColumnComment
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_SCHEMA = @db AND TABLE_NAME = @table
                        ORDER BY ORDINAL_POSITION;";
            return (await Connection.QueryAsync<ColumnInfo>(sql, new { db = Connection.Database , table = table })).ToList();
        }

        /// <summary>
        /// 查询未生成过的表
        /// </summary>
        /// <returns></returns>
        public async Task<EntityPaged<TableInfo>> QueryTablesAsync(int current, int pageSize, string? tableName)
        {
            var sql = @"SELECT 
                            S.TABLE_NAME AS TableName,
                            S.TABLE_COMMENT AS TableComment,
                            S.CREATE_TIME AS CreateTime,
                            S.UPDATE_TIME AS UpdateTime
                        FROM INFORMATION_SCHEMA.TABLES AS S
                        WHERE S.TABLE_SCHEMA = @db AND S.TABLE_NAME NOT IN (SELECT table_name FROM gen_table) AND S.TABLE_NAME NOT IN ('cap.published','cap.received')";
            if (!string.IsNullOrEmpty(tableName))
            {
                sql += " AND S.TABLE_NAME like @tableName";
            }
            sql += " ORDER BY S.TABLE_NAME";
            return await Connection.QueryListFromSqlAsync<TableInfo>(current, pageSize, sql, new DynamicParameters(new { db = Connection.Database, tableName = $"%{tableName}%" }));
        }

        /// <summary>
        /// 查询表信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public async Task<TableInfo?> QueryTableAsync(string table)
        {
            var sql = @"SELECT 
                            S.TABLE_NAME AS TableName,
                            S.TABLE_COMMENT AS TableComment,
                            S.CREATE_TIME AS CreateTime,
                            S.UPDATE_TIME AS UpdateTime
                        FROM INFORMATION_SCHEMA.TABLES AS S
                        WHERE S.TABLE_SCHEMA = @db AND S.TABLE_NAME = @table
                        ORDER BY S.TABLE_NAME";
            return await Connection.QueryFirstAsync<TableInfo>(sql, new { db = Connection.Database , table = table });
        }
    }
}
