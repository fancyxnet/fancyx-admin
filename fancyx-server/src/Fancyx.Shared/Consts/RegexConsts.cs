namespace Fancyx.Shared.Consts
{
    public static class RegexConsts
    {
        /// <summary>
        /// 用户名（大小写字母，数字，下划线，长度3-12位）
        /// </summary>
        public const string UserName = "^[a-zA-Z0-9_]{3,12}$";

        /// <summary>
        /// 密码（至少有一个字母和数字，长度6-16位，特殊字符 "~`!@#$%^&*()_-+={[}]|\:;\"'<,>.?/）
        /// </summary>
        public const string Password = "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d~`!@#$%^&*()_\\-+={[}\\]|\\\\:;\"'<,>.?/]{6,16}$";
    }
}