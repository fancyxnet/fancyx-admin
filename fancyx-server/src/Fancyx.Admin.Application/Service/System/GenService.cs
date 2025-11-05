using AutoMapper;
using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.EfCore.Entities.Gen;
using Fancyx.Admin.EfCore.Repositories;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.EfCore.Aop;
using Fancyx.EfCore.BaseEntity;
using Fancyx.SnowflakeId;
using Fancyx.Utils;
using JinianNet.JNTemplate;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Fancyx.Admin.Application.Service.System
{
    public class GenService : IGenService
    {
        private readonly IRepository<GenTable> _genTableRepository;
        private readonly IRepository<GenTableColumn> _genTableColumnRepository;
        private readonly GenRepository _genRepository;
        private readonly IMapper _mapper;

        public GenService(IRepository<GenTable> genTableRepository, IRepository<GenTableColumn> genTableColumnRepository, GenRepository genRepository
            , IMapper mapper)
        {
            _genTableRepository = genTableRepository;
            _genTableColumnRepository = genTableColumnRepository;
            _genRepository = genRepository;
            _mapper = mapper;
        }

        private readonly string[] creationFields = ["creator_id", "creation_time"];
        private readonly string[] deletionFields = ["is_deleted", "deleter_id", "deletion_time"];
        private readonly string[] deletionFlagFields = ["is_deleted"];
        private readonly string[] modificationFields = ["last_modification_time", "last_modifier_id"];
        private readonly string[] treeFields = ["parent_id", "tree_path", "tree_level"];
        private readonly string[] tenantFields = ["tenant_id"];

        public async Task<GenCodeResultDto> GenCodeAsync(long tableId)
        {
            var result = new GenCodeResultDto();

            var genTable = await _genTableRepository.FindAsync(tableId) ?? throw new EntityNotFoundException();
            var genTableColumns = await _genTableColumnRepository.Where(x => x.TableId == tableId).ToListAsync();

            var iServiceTemplate = this.LoadTemplate("IService");
            iServiceTemplate.Set("namespace_name", genTable.NamespaceName);
            iServiceTemplate.Set("class_name", genTable.ClassName);
            iServiceTemplate.Set("table_comment", genTable.TableComment);
            iServiceTemplate.Set("module_name", genTable.ModuleName);
            iServiceTemplate.Set("business_name", genTable.BusinessName);
            iServiceTemplate.Set("function_name", genTable.FunctionName);
            result.IService = iServiceTemplate.Render();

            var entityTemplate = this.LoadTemplate("Entity");
            var propStrBuilder = new StringBuilder();
            var isPrimaryKeyOfId = genTableColumns.Any(x => x.IsPk && x.ColumnName == "id");
            var primaryKeyCount = genTableColumns.Where(x => x.IsPk).Count();
            if (primaryKeyCount == 1)
            {
                var primaryKeyType = genTableColumns.Find(x => x.IsPk)!.CsharpType!;
                var columns = genTableColumns.Select(x => x.ColumnName).ToList();
                var inheritClassOrInterfaces = this.GetInheritClassOrInterface(isPrimaryKeyOfId, primaryKeyType, columns!);
                entityTemplate.Set("inherit", $": {inheritClassOrInterfaces}");
            }
            foreach (var item in genTableColumns)
            {
                propStrBuilder.AppendLine("\t /// <summary>");
                propStrBuilder.AppendLine($"\t /// {item.ColumnComment}");
                propStrBuilder.AppendLine("\t /// </summary>");
                propStrBuilder.AppendLine($"\tpublic {item.CsharpType}{(item.IsRequired ? "?" : "")} {item.CsharpField}");
                propStrBuilder.Append(" { get; set; }\r\n");
            }
            entityTemplate.Set("namespace_name", genTable.NamespaceName);
            entityTemplate.Set("class_name", genTable.ClassName);
            entityTemplate.Set("table_comment", genTable.TableComment);
            entityTemplate.Set("properties", propStrBuilder.ToString());
            result.Entity = entityTemplate.Render();

            return result;
        }

        //[AsyncTransactional] TODO: 加上有BUG，参见 https://mysqlconnector.net/troubleshooting/transaction-usage/
        public async Task ImportTableAsync(string table)
        {
            if (await _genTableRepository.AnyAsync(x => x.TableName == table)) throw new BusinessException("表已生成");

            var tableInfo = await _genRepository.QueryTableAsync(table) ?? throw new BusinessException($"数据库表{table}不存在");
            var genTable = new GenTable()
            {
                TableId = IdGenerater.Instance.NextId(),
                TableName = tableInfo.TableName,
                TableComment = tableInfo.TableComment,
                ClassName = StringUtils.ToPascalCase(tableInfo.TableName),
                ModuleName = "System",
                FunctionName = tableInfo.TableComment
            };
            genTable.BusinessName = genTable.ClassName;
            await _genTableRepository.InsertAsync(genTable);

            var columnInfos = await _genRepository.QueryColumnsAsync(tableInfo.TableName);
            var genTableColumns = new List<GenTableColumn>();
            var sort = 1;
            foreach (var item in columnInfos)
            {
                var isDefaultField = this.IsDefaultFields(item.ColumnName);
                var genTableColumn = new GenTableColumn
                {
                    ColumnId = IdGenerater.Instance.NextId(),
                    TableId = genTable.TableId,
                    ColumnName = item.ColumnName,
                    ColumnComment = item.ColumnComment,
                    ColumnType = item.ColumnType,
                    CsharpType = this.MapToCSharpType(item.ColumnType),
                    CsharpField = StringUtils.ToPascalCase(item.ColumnName),
                    IsPk = item.ColumnKey == "PRI",
                    IsRequired = item.IsNullable == "YES",
                    IsInsert = !isDefaultField,
                    IsEdit = !isDefaultField,
                    IsList = !isDefaultField,
                    IsQuery = !isDefaultField && IsDefaultQuery(item.ColumnType, item.ColumnName),
                    HtmlType = "text",
                    QueryType = "=",
                    Sort = sort,
                };
                sort++;
                genTableColumns.Add(genTableColumn);
            }
            await _genTableColumnRepository.InsertManyAsync(genTableColumns);
        }

        public async Task<PagedResult<TableInfoDto>> GetTableListAsync(GetTableQueryDto dto)
        {
            var resp = await _genRepository.QueryTablesAsync(dto.Current, dto.PageSize);
            return new PagedResult<TableInfoDto>(resp.Total, _mapper.Map<List<TableInfoDto>>(resp.Items));
        }

        public Task GenSyncFromDb(long tableId)
        {
            throw new NotImplementedException();
        }

        private ITemplate LoadTemplate(string templateName)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gen", $"{templateName}.txt");
            return Engine.LoadTemplate(filePath);
        }

        private string MapToCSharpType(string columnType)
        {
            // columnType值如：varchar(16)
            if (columnType.Contains('('))
            {
                columnType = columnType[..columnType.IndexOf('(')];
            }

            return columnType switch
            {
                "bigint" => "long",
                "varchar" or "text" or "longtext" or "json" => "string",
                "int" => "int",
                "tinyint" => "int",
                "bit" => "boolean",
                "datetime" => "DateTime",
                "decimal" => "decimal",
                _ => throw new NotSupportedException($"不支持的列类型 => {columnType}"),
            };
        }

        private bool IsDefaultQuery(string columnType, string field)
        {
            if (columnType != "varchar") return false;

            var noQueryFields = new string[] { "remark" };
            return !noQueryFields.Contains(field);
        }

        private bool IsDefaultFields(string field)
        {
            return creationFields.Any(f => f == field) || deletionFields.Any(f => f == field) || deletionFlagFields.Any(f => f == field)
                || modificationFields.Any(f => f == field) || treeFields.Any(f => f == field) || tenantFields.Any(f => f == field);
        }

        private string GetInheritClassOrInterface(bool idIsPrimaryKey, string primaryKeyCsType, List<string> columns)
        {
            var arr = new List<string>();
            var hasCreationFields = creationFields.All(columns.Contains);
            var hasDeletionFields = deletionFields.All(columns.Contains);
            var hasDeletionFlagFields = deletionFlagFields.All(columns.Contains);
            var hasModificationFields = modificationFields.All(columns.Contains);
            var hasTreeFields = treeFields.All(columns.Contains);
            var hasTenantFields = tenantFields.All(columns.Contains);

            if (idIsPrimaryKey && hasCreationFields && hasModificationFields && hasDeletionFields) arr.Add($"FullAuditedEntity<{primaryKeyCsType}>");
            if (idIsPrimaryKey && hasCreationFields && hasModificationFields && !hasDeletionFields) arr.Add($"AuditedEntity<{primaryKeyCsType}>");
            if (idIsPrimaryKey && hasCreationFields && !hasModificationFields && !hasDeletionFields) arr.Add($"CreationEntity<{primaryKeyCsType}>");
            if (idIsPrimaryKey && !hasCreationFields && !hasModificationFields && !hasDeletionFields) arr.Add($"Entity<{primaryKeyCsType}>");
            if (hasDeletionFlagFields && !hasDeletionFields) arr.Add(nameof(IHasDeletionFlagProperty));
            if (!idIsPrimaryKey && hasCreationFields) arr.Add($"IHasCreationProperty<{primaryKeyCsType}>");
            if (!idIsPrimaryKey && hasModificationFields) arr.Add($"IHasModificationProperty<{primaryKeyCsType}>");
            if (hasTenantFields) arr.Add(nameof(ITenant));
            if (hasTreeFields) arr.Add($"IHasTreeProperty<{primaryKeyCsType}>");

            return string.Join(", ", arr);
        }
    }
}
