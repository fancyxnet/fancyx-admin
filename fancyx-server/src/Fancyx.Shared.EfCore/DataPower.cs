namespace Fancyx.Shared.EfCore
{
    /// <summary>
    /// 数据权限字段名标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataPowerAttribute : Attribute
    {
        public string Field { get; private set; }

        public DataPowerAttribute(string field)
        {
            ArgumentNullException.ThrowIfNull(field, nameof(field));
            Field = field;
        }
    }

    public static class DataPower
    {
        public const string UserId = "UserId";
        public const string DeptId = "DeptId";

        public const string UserIdType = "power_user_ids";
        public const string DeptIdType = "power_dept_ids";
    }
}