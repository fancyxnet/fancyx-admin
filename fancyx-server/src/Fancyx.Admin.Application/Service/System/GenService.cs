using AutoMapper;
using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.EfCore.Entities.Gen;
using Fancyx.Admin.EfCore.Repositories;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
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
            var genTableColumns = await _genTableColumnRepository.Where(x => x.TableId == tableId).AsNoTracking().ToListAsync();

            // IService
            var iServiceTemplate = this.LoadTemplate("IService", genTable);
            result.IService = new AppOption($"I{genTable.BusinessName}Service", iServiceTemplate.Render());

            // Controller
            var controllerTemplate = this.LoadTemplate("Controller", genTable);
            result.Controller = new AppOption($"{genTable.BusinessName}Controller", controllerTemplate.Render());

            // Entity
            var entityTemplate = this.LoadTemplate("Entity", genTable);
            var isPrimaryKeyOfId = genTableColumns.Any(x => x.IsPk && x.ColumnName == "id");
            var primaryKeyCount = genTableColumns.Where(x => x.IsPk).Count();
            List<string> exceptFields = [];
            if (primaryKeyCount == 1)
            {
                var primaryKeyType = genTableColumns.Find(x => x.IsPk)!.CsharpType!;
                var columns = genTableColumns.Select(x => x.ColumnName).ToList();
                (string inheritClassOrInterfaces, exceptFields) = this.GetInheritClassOrInterface(isPrimaryKeyOfId, primaryKeyType, columns!);
                entityTemplate.Set("inherit", $": {inheritClassOrInterfaces}");
            }
            var entityPropStrBuilder = new StringBuilder();
            var businessAddDtoPropStrBuilder = new StringBuilder();
            var businessUpdateDtoPropStrBuilder = new StringBuilder();
            var businessListDtoPropStrBuilder = new StringBuilder();
            var businessQueryDtoPropStrBuilder = new StringBuilder();
            foreach (var item in genTableColumns)
            {
                if (!exceptFields.Contains(item.ColumnName!)) this.AddProperties(entityPropStrBuilder, item);
                if (item.IsInsert) this.AddProperties(businessAddDtoPropStrBuilder, item);
                if (item.IsEdit) this.AddProperties(businessUpdateDtoPropStrBuilder, item);
                if (item.IsList) this.AddProperties(businessListDtoPropStrBuilder, item);
                if (item.IsQuery) this.AddProperties(businessQueryDtoPropStrBuilder, item);
            }
            entityTemplate.Set("properties", entityPropStrBuilder.ToString());
            result.Entity = new AppOption(genTable.ClassName!, entityTemplate.Render());

            // AddDto
            var businessAddDtoTemplate = this.LoadTemplate("Dto", genTable);
            var addDtoName = $"{genTable.BusinessName}AddDto";
            businessAddDtoTemplate.Set("properties", businessAddDtoPropStrBuilder);
            businessAddDtoTemplate.Set("dto_name", addDtoName);
            result.BusinessAddDto = new AppOption(addDtoName, businessAddDtoTemplate.Render());

            // UpdateDto
            var businessUpdateDtoTemplate = this.LoadTemplate("Dto", genTable);
            var updateDtoName = $"{genTable.BusinessName}UpdateDto";
            businessUpdateDtoTemplate.Set("properties", businessUpdateDtoPropStrBuilder);
            businessUpdateDtoTemplate.Set("dto_name", updateDtoName);
            result.BusinessUpdateDto = new AppOption(updateDtoName, businessUpdateDtoTemplate.Render());

            // ListDto
            var businessListDtoTemplate = this.LoadTemplate("Dto", genTable);
            var listDtoName = $"{genTable.BusinessName}ListDto";
            businessListDtoTemplate.Set("properties", businessListDtoPropStrBuilder);
            businessListDtoTemplate.Set("dto_name", listDtoName);
            result.BusinessListDto = new AppOption(listDtoName, businessListDtoTemplate.Render());

            // Dto
            var businessDtoTemplate = this.LoadTemplate("Dto", genTable);
            var dtoName = $"{genTable.BusinessName}Dto";
            businessDtoTemplate.Set("properties", businessListDtoPropStrBuilder);
            businessDtoTemplate.Set("dto_name", dtoName);
            result.BusinessDto = new AppOption(dtoName, businessDtoTemplate.Render());

            // QueryDto
            var businessQueryDtoTemplate = this.LoadTemplate("Dto", genTable);
            var queryDtoName = $"{genTable.BusinessName}QueryDto";
            businessQueryDtoTemplate.Set("properties", businessQueryDtoPropStrBuilder);
            businessQueryDtoTemplate.Set("dto_name", queryDtoName);
            result.BusinessQueryDto = new AppOption(queryDtoName, businessQueryDtoTemplate.Render());

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
                var isPk = item.ColumnKey == "PRI";
                var genTableColumn = new GenTableColumn
                {
                    ColumnId = IdGenerater.Instance.NextId(),
                    TableId = genTable.TableId,
                    ColumnName = item.ColumnName,
                    ColumnComment = item.ColumnComment,
                    ColumnType = item.ColumnType,
                    CsharpType = this.MapToCSharpType(item.ColumnType),
                    CsharpField = StringUtils.ToPascalCase(item.ColumnName),
                    IsPk = isPk,
                    IsRequired = item.IsNullable == "YES",
                    IsInsert = !isDefaultField,
                    IsEdit = !isDefaultField || isPk,
                    IsList = !isDefaultField || isPk,
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

        private StringBuilder AddProperties(StringBuilder sb, GenTableColumn item, bool? isNullable = null)
        {
            isNullable ??= item.IsRequired;
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine($"\t/// {item.ColumnComment}");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine($"\tpublic {item.CsharpType}{(isNullable.Value ? "?" : "")} {item.CsharpField}" + " { get; set; } \r\n");
            return sb;
        }

        private ITemplate LoadTemplate(string templateName, GenTable genTable)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gen", $"{templateName}.txt");
            var template = Engine.LoadTemplate(filePath);

            template.Set("table_name", genTable.TableName);
            template.Set("table_comment", genTable.TableComment);
            template.Set("class_name", genTable.ClassName);
            template.Set("namespace_name", genTable.NamespaceName);
            template.Set("module_name", genTable.ModuleName);
            template.Set("business_name", genTable.BusinessName);
            template.Set("function_name", genTable.FunctionName);
            var businessNameInject = genTable.BusinessName?.Length > 1 ? genTable.BusinessName[..1].ToLowerInvariant() + genTable.BusinessName[1..] : genTable.BusinessName?.ToLowerInvariant();
            template.Set("business_name_inject", businessNameInject);

            return template;
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

            var noQueryFields = new string[] { "remark", "content", "img", "file", "image", "url", "link" };
            return !noQueryFields.Contains(field);
        }

        private bool IsDefaultFields(string field)
        {
            return creationFields.Any(f => f == field) || deletionFields.Any(f => f == field) || deletionFlagFields.Any(f => f == field)
                || modificationFields.Any(f => f == field) || tenantFields.Any(f => f == field);
        }

        private (string inheritClassOrInterfaces, List<string> exceptFields) GetInheritClassOrInterface(bool idIsPrimaryKey, string primaryKeyCsType, List<string> columns)
        {
            var arr = new List<string>();
            var exceptFields = new HashSet<string>();
            var hasCreationFields = creationFields.All(columns.Contains);
            var hasDeletionFields = deletionFields.All(columns.Contains);
            var hasDeletionFlagFields = deletionFlagFields.All(columns.Contains);
            var hasModificationFields = modificationFields.All(columns.Contains);
            var hasTreeFields = treeFields.All(columns.Contains);
            var hasTenantFields = tenantFields.All(columns.Contains);

            if (idIsPrimaryKey && hasCreationFields && hasModificationFields && hasDeletionFields)
            {
                arr.Add($"FullAuditedEntity<{primaryKeyCsType}>");
                AddExceptFields(["id"], creationFields, modificationFields, deletionFields);
            }
            if (idIsPrimaryKey && hasCreationFields && hasModificationFields && !hasDeletionFields)
            {
                arr.Add($"AuditedEntity<{primaryKeyCsType}>");
                AddExceptFields(["id"], creationFields, modificationFields);
            }
            if (idIsPrimaryKey && hasCreationFields && !hasModificationFields && !hasDeletionFields)
            {
                arr.Add($"CreationEntity<{primaryKeyCsType}>");
                AddExceptFields(["id"], creationFields);
            }
            if (idIsPrimaryKey && !hasCreationFields && !hasModificationFields && !hasDeletionFields)
            {
                arr.Add($"Entity<{primaryKeyCsType}>");
                AddExceptFields(["id"]);
            }
            if (hasDeletionFlagFields && !hasDeletionFields)
            {
                arr.Add(nameof(IHasDeletionFlagProperty));
                AddExceptFields(deletionFlagFields);
            }
            if (!idIsPrimaryKey && hasCreationFields)
            {
                arr.Add($"IHasCreationProperty<{primaryKeyCsType}>");
                AddExceptFields(creationFields);
            }
            if (!idIsPrimaryKey && hasModificationFields)
            {
                arr.Add($"IHasModificationProperty<{primaryKeyCsType}>");
                AddExceptFields(modificationFields);
            }
            if (hasTenantFields)
            {
                arr.Add(nameof(ITenant));
                AddExceptFields(tenantFields);
            }
            if (hasTreeFields)
            {
                arr.Add($"IHasTreeProperty<{primaryKeyCsType}>");
                AddExceptFields(treeFields);
            }

            void AddExceptFields(params string[][] fields)
            {
                foreach (var subFields in fields)
                {
                    foreach (var field in subFields)
                    {
                        exceptFields.Add(field);
                    }
                }
            }

            return (string.Join(", ", arr), exceptFields.ToList());
        }
    }
}
