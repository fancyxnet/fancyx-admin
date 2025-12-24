using AutoMapper;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
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

        public async Task<GenCodeResponse> GenCodeAsync(long tableId)
        {
            var result = new GenCodeResponse();

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
            var pageInsertFormItems = new StringBuilder("{ !isEdit && <>");
            var pageUpdateFormItems = new StringBuilder("{ isEdit && <>");
            var pageInsertAndUpdateItems = new StringBuilder();
            foreach (var item in genTableColumns)
            {
                var cameField = StringUtils.ToFirstLetterLowerCase(item.CsharpField!);
                if (!exceptFields.Contains(item.ColumnName!)) this.AddProperties(entityPropStrBuilder, item);
                this.AddTsProperties(tsFieldStrBuilder, item, cameField);
                if (item.IsQuery)
                {
                    pageSearchItemsStrBuilder.Append(this.BuildAntdFormItem(item.ColumnComment!, item.ColumnName!, item.HtmlType!, item.ColumnName!));
                    this.AddProperties(queryPropStrBuilder, item, true, item.QueryType == "BETWEEN");
                    this.AddTsProperties(tsFieldQueryStrBuilder, item, cameField, true, item.QueryType == "BETWEEN");
                    if (item.CsharpType == "string")
                    {
                        queryConditionPropStrBuilder.Append($".WhereIf(!string.IsNullOrEmpty(req.{item.CsharpField}), x => ");
                        if (item.QueryType == "LIKE")
                        {
                            queryConditionPropStrBuilder.Append($"x.StartWith(req.{item.CsharpField})");
                        }
                    }
                    else
                    {
                        queryConditionPropStrBuilder.Append($".WhereIf({item.CsharpField}.HasValue, x => ");
                        if (item.QueryType == ">" || item.QueryType == ">=" || item.QueryType == "<"
                            || item.QueryType == "<=")
                        {
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} {item.QueryType} req.{item.CsharpField}");
                        }
                    }
                    switch (item.QueryType)
                    {
                        case "=":
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} == req.{item.CsharpField}");
                            break;
                        case "!=":
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} != req.{item.CsharpField}");
                            break;
                        case "BETWEEN":
                            queryConditionPropStrBuilder.Append($"x.{item.CsharpField} between req.{item.CsharpField}[0] and req.{item.CsharpField}[1]");
                            break;
                    }
                    queryConditionPropStrBuilder.Append(')');
                }
                if (item.IsList)
                {
                    listAssignPropStrBuilder.AppendLine($"\t\t{item.CsharpField} = x.{item.CsharpField},");
                    pageColumnsStrBuilder.Append(this.BuildAntdColumnItem(item.ColumnComment!, cameField, item.IsQuery));
                }
                bool isInsertAndUpdate = item.IsInsert && item.IsEdit;
                var formItem = this.BuildAntdFormItem(item.ColumnComment!, cameField, item.HtmlType!, item.ColumnName!, isRequired: item.IsRequired);
                if (isInsertAndUpdate)
                {
                    pageInsertAndUpdateItems.Append(formItem);
                }
                if (item.IsInsert)
                {
                    addAssignPropStrBuilder.AppendLine($"\t{item.CsharpField} = req.{item.CsharpField},");
                    if (!isInsertAndUpdate) pageInsertFormItems.Append(formItem);
                }
                if (item.IsEdit)
                {
                    updateAssignPropStrBuilder.AppendLine($"\tentity.{item.CsharpField} = req.{item.CsharpField};");
                    if (!isInsertAndUpdate) pageUpdateFormItems.Append(formItem);
                }
            }

            if (pageInsertFormItems.Length < 20)
            {
                pageInsertFormItems.Clear();
            }
            else
            {
                pageInsertFormItems.Append(" </> } ");
            }
            if (pageUpdateFormItems.Length < 20)
            {
                pageUpdateFormItems.Clear();
            }
            else
            {
                pageUpdateFormItems.Append(" </> } ");
            }

            entityTemplate.Set("properties", entityPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.Entity = new AppOption($"{genTable.ClassName}.cs", entityTemplate.Render());

            // Service
            var serviceTemplate = this.LoadTemplate("Service", genTable);
            serviceTemplate.Set("listProperties", listAssignPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            serviceTemplate.Set("addProperties", addAssignPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            serviceTemplate.Set("updateProperties", updateAssignPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            serviceTemplate.Set("queryConditions", queryConditionPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.Service = new AppOption($"{genTable.BusinessName}Service.cs", serviceTemplate.Render());

            // ListRequest
            var queryDtoTemplate = this.LoadTemplate("ListRequest", genTable);
            queryDtoTemplate.Set("properties", queryPropStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.QueryDto = new AppOption($"Get{genTable.BusinessName}ListRequest.cs", queryDtoTemplate.Render());

            // Api
            var apiTemplate = this.LoadTemplate("api", genTable);
            apiTemplate.Set("ts_interface_fields", tsFieldStrBuilder.ToString().TrimEnd('\r', '\n'));
            apiTemplate.Set("ts_interface_query_fields", tsFieldQueryStrBuilder.ToString().TrimEnd('\r', '\n'));
            result.Api = new AppOption("api.ts", apiTemplate.Render());

            // Page
            var pageTemplate = this.LoadTemplate("page", genTable);
            pageInsertAndUpdateItems.Append(pageUpdateFormItems.Append(pageInsertFormItems));
            pageTemplate.Set("searchItems", pageSearchItemsStrBuilder.ToString());
            pageTemplate.Set("operationFormItems", pageInsertAndUpdateItems.ToString());
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

        public async Task<PagedResult<TableInfoItem>> GetTableListAsync(GetTableListRequest req)
        {
            var resp = await _genRepository.QueryTablesAsync(req.Current, req.PageSize, req.TableName);
            return new PagedResult<TableInfoItem>(resp.Total, _mapper.Map<List<TableInfoItem>>(resp.Items));
        }

        public async Task<PagedResult<GenTableItem>> GetGenTableListAsync(GetGenTableListRequest req)
        {
            var resp = await _genTableRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(req.TableName), x => x.TableName != null && x.TableName.StartsWith(req.TableName!))
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<GenTableItem>(resp.Total, _mapper.Map<List<GenTableItem>>(resp.Items));
        }

        public async Task<PagedResult<GenTableColumnItem>> GetGenTableColumnListAsync(GenTableColumnRequest req)
        {
            var resp = await _genTableColumnRepository.Where(x => x.TableId == req.TableId)
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<GenTableColumnItem>(resp.Total, _mapper.Map<List<GenTableColumnItem>>(resp.Items));
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

        public async Task SaveGenTableInfoAsync(SaveGenTableInfoRequest req)
        {
            var genTable = await _genTableRepository.FindAsync(req.TableId) ?? throw new EntityNotFoundException();
            genTable.TableId = req.TableId;
            genTable.TableComment = req.TableComment;
            genTable.ClassName = req.ClassName;
            genTable.TplCategory = req.TplCategory;
            genTable.NamespaceName = req.NamespaceName;
            genTable.ModuleName = req.ModuleName;
            genTable.BusinessName = req.BusinessName;
            genTable.FunctionName = req.FunctionName;
            genTable.GenPath = req.GenPath;
            genTable.Options = req.Options;
            genTable.Remark = req.Remark;
            await _genTableRepository.UpdateAsync(genTable);
        }

        [AsyncTransactional]
        public async Task SaveGenColumnInfoAsync(List<SaveGenColumnInfoItem> dtos)
        {
            var columnIds = dtos.Select(x => x.ColumnId).ToList();
            var genTableColumns = await _genTableColumnRepository.AsNoTracking().Where(x => columnIds.Contains(x.ColumnId)).ToListAsync();
            foreach (var req in dtos)
            {
                var entity = genTableColumns.Find(x => x.ColumnId == req.ColumnId);
                if (entity == null) continue;
                entity.ColumnName = req.ColumnName;
                entity.ColumnComment = req.ColumnComment;
                entity.ColumnType = req.ColumnType;
                entity.CsharpType = req.CsharpType;
                entity.TsType = req.TsType;
                entity.CsharpField = req.CsharpField;
                entity.IsPk = req.IsPk;
                entity.IsIncrement = req.IsIncrement;
                entity.IsRequired = req.IsRequired;
                entity.IsInsert = req.IsInsert;
                entity.IsEdit = req.IsEdit;
                entity.IsList = req.IsList;
                entity.IsQuery = req.IsQuery;
                entity.QueryType = req.QueryType;
                entity.HtmlType = req.HtmlType;
                entity.DictType = req.DictType;
                entity.Sort = req.Sort;
            }
            await _genTableColumnRepository.UpdateManyAsync(genTableColumns);
        }

        public async Task<GenDetails> GetGenDetailsInfoAsync(long tableId)
        {
            var genTable = await _genTableRepository.FindAsync(tableId) ?? throw new EntityNotFoundException();
            return _mapper.Map<GenDetails>(genTable);
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
            var defaultValue = !isNullable == true && item.CsharpType == "string" ? " = null!;" : "";
            if (!isNullable == true)
            {
                sb.AppendLine("\t[Required]");
            }
            sb.AppendLine($"\tpublic {item.CsharpType}{(isNullable.Value ? "?" : "")} {item.CsharpField}{(isArray ? "[]" : "")}" + " { get; set; } " + $"{defaultValue} \r\n");
            return sb;
        }

        private StringBuilder AddTsProperties(StringBuilder sb, GenTableColumn item, string field, bool? isNullable = null, bool isArray = false)
        {
            if (this.IsDefaultFields(item.ColumnName!)) return sb;

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
                    if (lastDotIndex < genTable.ModuleName.Length - 1)
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
            // 移除长度、精度等括号内的信息，并转换为小写便于比较
            string baseType = columnType.ToLower();
            if (baseType.Contains('('))
            {
                baseType = baseType[..baseType.IndexOf('(')].Trim();
            }

            // 处理无符号类型
            bool isUnsigned = baseType.Contains("unsigned");
            if (isUnsigned)
            {
                baseType = baseType.Replace("unsigned", "").Trim();
            }

            return baseType switch
            {
                // 整数类型
                "bigint" => isUnsigned ? "ulong" : "long",
                "int" or "integer" => isUnsigned ? "uint" : "int",
                "mediumint" => isUnsigned ? "uint" : "int",
                "smallint" => isUnsigned ? "ushort" : "short",
                "tinyint" => "int", // MySQL中tinyint(1)通常表示bool，但其他情况表示byte
                "bit" => "bool",

                // 浮点类型
                "decimal" or "dec" or "numeric" => "decimal",
                "float" => "float",
                "double" or "real" => "double",

                // 字符串类型
                "varchar" or "char" or "text" or "tinytext"
                    or "mediumtext" or "longtext" or "json"
                    or "enum" or "set" => "string",

                // 二进制类型
                "binary" or "varbinary" or "blob" or "tinyblob"
                    or "mediumblob" or "longblob" => "byte[]",

                // 日期时间类型
                "date" => "DateTime",
                "datetime" => "DateTime",
                "timestamp" => "DateTime",
                "time" => "TimeSpan",
                "year" => "int",

                // 空间数据类型
                "geometry" or "point" or "linestring" or "polygon"
                    or "multipoint" or "multilinestring" or "multipolygon"
                    or "geometrycollection" => "byte[]", // 或者使用特定的空间类型如MySqlGeometry

                // 其他类型
                "bool" or "boolean" => "bool",

                _ => throw new NotSupportedException($"不支持的列类型: {columnType}"),
            };
        }

        private string MapToTsType(string columnType)
        {
            // 移除长度、精度等括号内的信息，并转换为小写便于比较
            string baseType = columnType.ToLower();
            if (baseType.Contains('('))
            {
                baseType = baseType[..baseType.IndexOf('(')].Trim();
            }

            // 处理无符号类型
            bool isUnsigned = baseType.Contains("unsigned");
            if (isUnsigned)
            {
                baseType = baseType.Replace("unsigned", "").Trim();
            }

            return baseType switch
            {
                // 整数类型
                "int" or "integer" or "mediumint" or "smallint" or "tinyint" => "number",

                // 浮点类型
                "decimal" or "dec" or "numeric" or "float" or "double" or "real" => "number",

                // 字符串类型
                "bigint" or "varchar" or "char" or "text" or "tinytext" or "mediumtext"
                    or "longtext" or "json" or "enum" or "set" => "string",

                // 二进制类型 - 在前端通常用string表示(base64)或忽略
                "binary" or "varbinary" or "blob" or "tinyblob"
                    or "mediumblob" or "longblob" => "string", // 或者 "Uint8Array"

                // 布尔类型
                "bit" or "bool" or "boolean" => "boolean",

                // 日期时间类型
                "date" or "datetime" or "timestamp" => "Date",
                "time" => "string", // TimeSpan在TS中没有直接对应类型
                "year" => "number",

                // 空间数据类型 - 通常在前端不需要处理或作为string
                "geometry" or "point" or "linestring" or "polygon"
                    or "multipoint" or "multilinestring" or "multipolygon"
                    or "geometrycollection" => "string", // 或者特定的接口类型

                _ => "any", // 对于未知类型返回any而不是抛出异常
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
                html += string.Format(@" key=""{0}""", key);
            }
            var hasRule = isRequired.GetValueOrDefault() == true || len.GetValueOrDefault() > 0;
            if (hasRule)
            {
                html += " rules={[";
                if (isRequired == true)
                {
                    html += "{ required: true }, ";
                }
                if (len > 0)
                {
                    html += "{ max: 256 }, ";
                }
                html += "]}";
            }
            html += " >";
            if (htmlType == "text")
            {
                html += string.Format(@"<Input placeholder=""请输入{0}"" />", label);
            }
            if (htmlType == "textarea")
            {
                html += string.Format(@"<TextArea placeholder=""请输入{0}"" />", label);
            }

            html += "</Form.Item>";
            return html.ToString();
        }

        private string BuildAntdColumnItem(string title, string dataIndex, bool isSearch)
        {
            if (dataIndex.Equals("id", StringComparison.InvariantCultureIgnoreCase)) return string.Empty;
            var html = @"{
                  title: '" + title + @"',
                  dataIndex: '" + dataIndex + @"',"+ 
                  (!isSearch ? "\r\nsearch: false," : "") + @"
                },";
            return html;
        }
    }
}
