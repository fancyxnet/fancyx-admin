namespace Fancyx.DataAccess.Enums
{
    public enum DeptPowerType
    {
        /// <summary>
        /// 全部数据权限
        /// </summary>
        All,

        /// <summary>
        /// 本部门数据权限
        /// </summary>
        ThisLevel,

        /// <summary>
        /// 本部门及以下数据权限
        /// </summary>
        ThisLevelAndBelow,

        /// <summary>
        /// 指定部门数据权限
        /// </summary>
        Specify,

        /// <summary>
        /// 仅本人数据权限
        /// </summary>
        OnlyMe
    }
}