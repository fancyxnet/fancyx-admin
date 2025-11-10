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
using Microsoft.Extensions.Primitives;
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
            result.IService = new AppOption($"I{genTable.BusinessName}Service.cs", iServiceTemplate.Render());

            // Controller
            var controllerTemplate = this.LoadTemplate("Controller", genTable);
            result.Controller = new AppOption($"{genTable.BusinessName}Controller.cs", controllerTemplate.Render());

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
            var queryPropStrBuilder = new StringBuilder();
            var listAssignPropStrBuilder = new StringBuilder();
            var addAssignPropStrBuilder = new StringBuilder();
            var updateAssignPropStrBuilder = new StringBuilder();
            var queryConditionPropStrBuilder = new StringBuilder();
            var tsFieldStrBuilder = new StringBuilder();
            var tsFieldQueryStrBuilder = new StringBuilder();
            var pageColumnsStrBuilder = new StringBuilder();
            var pageSearchItemsStrBuilder = new StringBuilder();
            var pageInsertFormItems = new StringBuilder("{{ isEdit && <>");
            var pageUpdateFormItems = new StringBuilder("{{ !isEdit && <>");
            foreach (var item in genTableColumns)
            {
                if (!exceptFields.Contains(item.ColumnName!)) this.AddProperties(entityPropStrBuilder, item);
                this.AddTsProperties(tsFieldStrBuilder, item);
                if (item.IsQuery)
                {
                    pageSearchItemsStrBuilder.Append(this.BuildAntdFormItem(item.ColumnComment!, item.ColumnName!, item.HtmlType!, item.ColumnName!));
                    this.AddProperties(queryPropStrBuilder, item, true, item.QueryType == "BETWEEN");
                    this.AddTsProperties(tsFieldQueryStrBuilder, item, true, item.QueryType == "BETWEEN");
                    if(item.CsharpType == "string")
                    {
                        queryConditionPropStrBuilder.Append($".WhereIf(!string.IsNullOrEmpty(dto.{item.CsharpField}), x => ");
                        if(item.QueryType == "LIKE")
                        {
                            queryConditionPropStrBuilder.Append($"x.StartWith(dto.{item.CsharpField})");
                        }
                    }
                    else
                    {
                        queryConditionPropStrBuilder.Append($".WhereIf({item.CsharpField}.HasValue, x => ");
                        if(item.QueryType == ">" ||  item.QueryType == ">=" || item.QueryType == "<"
                            || item.QueryType == "<=")
                        {
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} {item.QueryType} dto.{item.CsharpField}");
                        }
                    }
                    switch (item.QueryType)
                    {
                        case "=":
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} == dto.{item.CsharpField}");
                            break;
                        case "!=":
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} != dto.{item.CsharpField}");
                            break;
                        case "BETWEEN":
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} between dto.{item.CsharpField}[0] and dto.{item.CsharpField}[1]");
                            break;
                    }
                    queryConditionPropStrBuilder.Append(')');
                }
                if (item.IsList)
                {
                    listAssignPropStrBuilder.AppendLine($"\t\t\t{item.CsharpField} = x.{item.CsharpField}");
                    pageColumnsStrBuilder.Append(this.BuildAntdColumnItem(item.ColumnComment!, item.ColumnName!));
                }
                if (item.IsInsert)
                {
                    addAssignPropStrBuilder.AppendLine($"\t\tentity.{item.CsharpField} = dto.{item.CsharpField}");
                    pageUpdateFormItems.Append(this.BuildAntdFormItem(item.ColumnComment!, item.ColumnName!, item.HtmlType!, item.ColumnName!, isRequired: item.IsRequired));
                }
                if (item.IsEdit)
                {
                    updateAssignPropStrBuilder.AppendLine($"\t    entity.{item.CsharpField} = dto.{item.CsharpField}");
                    pageUpdateFormItems.Append(this.BuildAntdFormItem(item.ColumnComment!, item.ColumnName!, item.HtmlType!, item.ColumnName!, isRequired: item.IsRequired));
                }
            }

            pageInsertFormItems.Append("}} </>");
            pageUpdateFormItems.Append("}} </>");

            entityTemplate.Set("properties", entityPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.Entity = new AppOption($"{genTable.ClassName}.cs", entityTemplate.Render());

            // Service
            var serviceTemplate = this.LoadTemplate("Service", genTable);
            serviceTemplate.Set("listProperties", listAssignPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            serviceTemplate.Set("addProperties", addAssignPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            serviceTemplate.Set("updateProperties", updateAssignPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            serviceTemplate.Set("queryConditions", queryConditionPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.Service = new AppOption($"{genTable.BusinessName}Service.cs", serviceTemplate.Render());

            // QueryDto
            var queryDtoTemplate = this.LoadTemplate("QueryDto", genTable);
            queryDtoTemplate.Set("properties", queryPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.QueryDto = new AppOption($"{genTable.BusinessName}QueryDto.cs", queryDtoTemplate.Render());

            // Api
            var apiTemplate = this.LoadTemplate("api", genTable);
            apiTemplate.Set("ts_interface_fields", tsFieldStrBuilder.ToString().TrimEnd('\r', '\n'));
            apiTemplate.Set("ts_interface_query_fields", tsFieldQueryStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.Api = new AppOption("api.ts", apiTemplate.Render());

            // Page
            var pageTemplate = this.LoadTemplate("page", genTable);
            var operationFormStr = pageUpdateFormItems.Append(pageInsertFormItems).ToString();
            pageTemplate.Set("searchItems", pageSearchItemsStrBuilder.ToString());
            pageTemplate.Set("operationFormItems", operationFormStr);
            pageTemplate.Set("columns", pageColumnsStrBuilder.ToString());
            result.Page = new AppOption("page.tsx", pageTemplate.Render());

            return result;
        }

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

            await this.InsertGenTableColumnsFromDb(genTable.TableId, tableInfo.TableName);
        }

        public async Task<PagedResult<TableInfoDto>> GetTableListAsync(GetTableQueryDto dto)
        {
            var resp = await _genRepository.QueryTablesAsync(dto.Current, dto.PageSize, dto.TableName);
            return new PagedResult<TableInfoDto>(resp.Total, _mapper.Map<List<TableInfoDto>>(resp.Items));
        }

        public async Task<PagedResult<GenTableListDto>> GetGenTableListAsync(GenTableQueryDto dto)
        {
            var resp = await _genTableRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.TableName), x => x.TableName != null && x.TableName.StartsWith(dto.TableName!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<GenTableListDto>(resp.Total, _mapper.Map<List<GenTableListDto>>(resp.Items));
        }

        public async Task<PagedResult<GenTableColumnListDto>> GetGenTableColumnListAsync(GenTableColumnQueryDto dto)
        {
            var resp = await _genTableColumnRepository.Where(x => x.TableId == dto.TableId)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<GenTableColumnListDto>(resp.Total, _mapper.Map<List<GenTableColumnListDto>>(resp.Items));
        }

        public async Task GenSyncFromDb(long tableId)
        {
            var genTable = await _genTableRepository.FindAsync(tableId) ?? throw new EntityNotFoundException();
            var tableInfo = await _genRepository.QueryTableAsync(genTable.TableName!) ?? throw new BusinessException($"数据库表{genTable.TableName}不存在");
            
            genTable.TableName = tableInfo.TableName;
            genTable.TableComment = tableInfo.TableComment;
            genTable.ClassName = StringUtils.ToPascalCase(tableInfo.TableName);
            await _genTableRepository.UpdateAsync(genTable);

            await this.InsertGenTableColumnsFromDb(genTable.TableId, tableInfo.TableName);
        }

        [AsyncTransactional]
        public async Task DeleteGenTableAsync(long tableId)
        {
            await _genTableColumnRepository.DeleteAsync(x => x.TableId == tableId);
            await _genTableRepository.DeleteAsync(x => x.TableId == tableId);
        }

        public async Task SaveGenTableInfoAsync(GenTableInfoDto dto)
        {
            var genTable = await _genTableRepository.FindAsync(dto.TableId) ?? throw new EntityNotFoundException();
            genTable.TableId = dto.TableId;
            genTable.TableComment = dto.TableComment;
            genTable.ClassName = dto.ClassName;
            genTable.TplCategory = dto.TplCategory;
            genTable.NamespaceName = dto.NamespaceName;
            genTable.ModuleName = dto.ModuleName;
            genTable.BusinessName = dto.BusinessName;
            genTable.FunctionName = dto.FunctionName;
            genTable.GenPath = dto.GenPath;
            genTable.Options = dto.Options;
            genTable.Remark = dto.Remark;
            await _genTableRepository.UpdateAsync(genTable);
        }

        [AsyncTransactional]
        public async Task SaveGenColumnInfoAsync(List<GenTableColumnDto> dtos)
        {
            var columnIds = dtos.Select(x => x.ColumnId).ToList();
            var genTableColumns = await _genTableColumnRepository.AsNoTracking().Where(x => columnIds.Contains(x.ColumnId)).ToListAsync();
            foreach (var dto in dtos)
            {
                var entity = genTableColumns.Find(x => x.ColumnId == dto.ColumnId);
                if (entity == null) continue;
                entity.ColumnName = dto.ColumnName;
                entity.ColumnComment = dto.ColumnComment;
                entity.ColumnType = dto.ColumnType;
                entity.CsharpType = dto.CsharpType;
                entity.TsType = dto.TsType;
                entity.CsharpField = dto.CsharpField;
                entity.IsPk = dto.IsPk;
                entity.IsIncrement = dto.IsIncrement;
                entity.IsRequired = dto.IsRequired;
                entity.IsInsert = dto.IsInsert;
                entity.IsEdit = dto.IsEdit;
                entity.IsList = dto.IsList;
                entity.IsQuery = dto.IsQuery;
                entity.QueryType = dto.QueryType;
                entity.HtmlType = dto.HtmlType;
                entity.DictType = dto.DictType;
                entity.Sort = dto.Sort;
            }
            await _genTableColumnRepository.UpdateManyAsync(genTableColumns);
        }

        public async Task<GenDetailsInfoDto> GetGenDetailsInfoAsync(long tableId)
        {
            var genTable = await _genTableRepository.FindAsync(tableId) ?? throw new EntityNotFoundException();
            return _mapper.Map<GenDetailsInfoDto>(genTable);
        }

        private async Task InsertGenTableColumnsFromDb(long tableId, string tableName)
        {
            await _genTableColumnRepository.DeleteAsync(x => x.TableId == tableId);
            var columnInfos = await _genRepository.QueryColumnsAsync(tableName);
            var genTableColumns = new List<GenTableColumn>();
            var sort = 1;
            foreach (var item in columnInfos)
            {
                var isDefaultField = this.IsDefaultFields(item.ColumnName);
                var isPk = item.ColumnKey == "PRI";
                var genTableColumn = new GenTableColumn
                {
                    ColumnId = IdGenerater.Instance.NextId(),
                    TableId = tableId,
                    ColumnName = item.ColumnName,
                    ColumnComment = item.ColumnComment,
                    ColumnType = item.ColumnType,
                    CsharpType = this.MapToCSharpType(item.ColumnType),
                    TsType = this.MapToTsType(item.ColumnType),
                    CsharpField = StringUtils.ToPascalCase(item.ColumnName),
                    IsPk = isPk,
                    IsRequired = item.IsNullable == "YES",
                    IsInsert = !isDefaultField,
                    IsEdit = !isDefaultField,
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

        private StringBuilder AddProperties(StringBuilder sb, GenTableColumn item, bool? isNullable = null, bool isArray = false)
        {
            isNullable ??= item.IsRequired;
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine($"\t/// {item.ColumnComment}");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine($"\tpublic {item.CsharpType}{(isNullable.Value ? "?" : "")} {item.CsharpField}{(isArray ? "[]": "")}" + " { get; set; } \r\n");
            return sb;
        }

        private StringBuilder AddTsProperties(StringBuilder sb, GenTableColumn item, bool? isNullable = null, bool isArray = false)
        {
            var field = StringUtils.ToFirstLetterLowerCase(item.CsharpField!);

            isNullable ??= item.IsRequired;
            sb.AppendLine($"  /** {item.ColumnComment} */");
            sb.AppendLine($"  {field}{(isArray ? "[]" : "")}:" + $" {item.TsType}{(isNullable.Value ? " | null" : "")};");
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
            var businessNameInject = StringUtils.ToFirstLetterLowerCase(genTable.BusinessName ?? "");
            template.Set("business_name_inject", businessNameInject);
            template.Set("business_name_lower", genTable.BusinessName?.ToLowerInvariant());
            template.Set("dir", genTable.NamespaceName?.ToLowerInvariant());

            var moduleNamePrefix = "xxx_module_name_prefix";
            if (!string.IsNullOrWhiteSpace(genTable.ModuleName))
            {
                if (genTable.ModuleName.Contains('.'))
                {
                    var lastDotIndex = genTable.ModuleName.LastIndexOf('.');
                    if(lastDotIndex < genTable.ModuleName.Length - 1)
                    {
                        moduleNamePrefix = genTable.ModuleName[(lastDotIndex + 1)..];
                    }
                }
            }
            template.Set("module_name_prefix", moduleNamePrefix);
            var moduleNameApiPrefix = $"{StringUtils.ToFirstLetterLowerCase(moduleNamePrefix)}";
            template.Set("module_name_api_prefix", moduleNameApiPrefix);

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
                "bit" => "bool",
                "datetime" => "DateTime",
                "decimal" => "decimal",
                _ => throw new NotSupportedException($"不支持的列类型 => {columnType}"),
            };
        }

        private string MapToTsType(string columnType)
        {
            // columnType值如：varchar(16)
            if (columnType.Contains('('))
            {
                columnType = columnType[..columnType.IndexOf('(')];
            }

            return columnType switch
            {
                "bigint" or "varchar" or "text" or "longtext" or "json" => "string",
                "int" or "tinyint" or "decimal" => "number",
                "bit" => "boolean",
                "datetime" => "Date",
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
            }
            if (!idIsPrimaryKey && hasCreationFields)
            {
                arr.Add($"IHasCreationProperty<{primaryKeyCsType}>");
                AddExceptFields(creationFields);
            }
            if (!idIsPrimaryKey && hasModificationFields)
            {
                arr.Add($"IHasModificationProperty<{primaryKeyCsType}>");
            }
            if (hasTenantFields)
            {
                arr.Add(nameof(ITenant));
            }
            if (hasTreeFields)
            {
                arr.Add($"IHasTreeProperty<{primaryKeyCsType}>");
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

        private string BuildAntdFormItem(string label, string name, string htmlType, string? key = null, bool? isRequired = false, int? len = 0)
        {
            string html = string.Format(@"<Form.Item label=""{0}"" name=""{1}""", label, name);
            if (!string.IsNullOrWhiteSpace(key))
            {
                html += string.Format(@"key=""{0}""", key);
            }
            html += " >";
            if(htmlType == "text")
            {
                html += string.Format(@"<Input placeholder=""请输入{0}"" />", label);
            }
            if (htmlType == "textarea")
            {
                html += string.Format(@"<TextArea placeholder=""请输入{0}"" />", label);
            }
            var hasRule = isRequired.GetValueOrDefault() == true || len.GetValueOrDefault() > 0;
            if (hasRule)
            {
                html += "{[";
                if(isRequired == true)
                {
                    html += "{ required: true }, ";
                }
                if(len > 0)
                {
                    html += "{ max: 256 }, ";
                }
                html += "]}";
            }

            html += "</Form.Item>";
            return html.ToString();
        }
    
        private string BuildAntdColumnItem(string title, string dataIndex)
        {
            var html = @"{
                  title: '" + title +@"',
                  dataIndex: '" + dataIndex +@"',
                },";
            return html;
        }
    }
}
