namespace Fancyx.Shared.Consts
{
    public static class LogRecordConsts
    {
        public const string User = "系统用户";
        public const string UserResetPwdSubType = "重置用户密码";
        public const string UserResetPwdContent = "重置用户{{userName}}登录密码";

        public const string DictType = "系统字典";
        public const string DictAddSubType = "新增字典";
        public const string DictAddContent = "新增字典{{dict.Name}}";
        public const string DictDeleteSubType = "删除字典";
        public const string DictDeleteContent = "删除字典：名称：{{dict.Name}}，类型：{{dict.DictType}}";
        public const string DictBatchDeleteSubType = "批量删除字典";
        public const string DictBatchDeleteContent = "批量删除ID为{{ids}}字典";

        public const string DictData = "字典数据";
        public const string DictDataUpdateSubType = "编辑字典数据";
        public const string DictDataUpdateContent = "编辑后：值={{after.Value}},启用={{after.IsEnabled}}";
        public const string DictDataDeleteSubType = "删除字典数据";
        public const string DictDataDeleteContent = "删除了{{ids}}字典项数据";
        
        public const string Config = "配置管理";
        public const string ConfigUpdateSubType = "编辑配置";
        public const string ConfigUpdateContent = "编辑后：键={{after.Key}}，值={{after.Value}}，组={{after.GroupKey}}";
    }
}