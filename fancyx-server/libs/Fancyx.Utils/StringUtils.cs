using System.Text;
using System.Text.RegularExpressions;

namespace Fancyx.Utils
{
    public static partial class StringUtils
    {
        /// <summary>
        /// 检查URL是否合法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsValidUrlStrict(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) && uriResult != null)
            {
                return uriResult.Scheme == Uri.UriSchemeHttp
                       || uriResult.Scheme == Uri.UriSchemeHttps
                       || uriResult.Scheme == Uri.UriSchemeFtp
                       || uriResult.Scheme == Uri.UriSchemeFtps;
            }

            return false;
        }

        /// <summary>
        /// 截断字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string? CutOff(string? str, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            if (str.Length > maxLength)
            {
                return str.Substring(0, maxLength);
            }
            return str;
        }

        /// <summary>
        /// 忽略大小写比较字符串值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool IgnoreCaseEquals(string? value1, string? value2)
        {
            return string.Equals(value1, value2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="len">字符长度</param>
        /// <param name="isNumber">是否纯数字</param>
        /// <param name="isWord">是否纯字母</param>
        /// <param name="hasChar">是否含特殊符号</param>
        /// <returns></returns>
        public static string RandomStr(int len, bool isNumber = false, bool isWord = false, bool hasChar = false)
        {
            var sb = new StringBuilder();
            if (isNumber)
            {
                Random r = new Random();
                for (int i = 0; i < len; i++)
                {
                    sb.Append(r.Next(0, 10));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 小写加下划线命名转帕斯卡命名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if(!input.Contains('_'))
            {
                var firstWord = input[..1].ToUpper();
                if (input.Length > 1) return firstWord + input[1..];
                return firstWord;
            }

            // 使用StringBuilder来构建结果字符串
            StringBuilder pascalCase = new StringBuilder();

            // 分割字符串
            string[] words = input.Split('_');

            // 遍历每个单词，并将其首字母大写
            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    pascalCase.Append(char.ToUpperInvariant(word[0]));
                    if (word.Length > 1)
                    {
                        pascalCase.Append(word.Substring(1).ToLower());
                    }
                }
            }

            return pascalCase.ToString();
        }

        public static string ToFirstLetterLowerCase(string input)
        {
            if(string.IsNullOrEmpty(input)) return input;
            return input.Length > 1 ? input[..1].ToLowerInvariant() + input[1..] : input.ToLowerInvariant();
        }

        public static string? XssFilte(string? input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // 使用正则表达式移除潜在的XSS攻击代码
            // 注意：以下正则表达式只是一个简单的例子，可能不会覆盖所有XSS攻击向量
            // 考虑使用HtmlSanitizer

            input = ScriptRegex().Replace(input, string.Empty);
            input = InputRegex().Replace(input, string.Empty);
            input = StyleRegex().Replace(input, string.Empty);
            input = IframeRegex().Replace(input, string.Empty);
            input = SrcRegex().Replace(input, "src=\"\"");

            return input;
        }

        /// <summary>
        /// 脱敏方法（如：替换为 *，保留最后几位）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keepLast"></param>
        /// <returns></returns>
        public static string MaskSensitiveData(string data, int keepLast = 4)
        {
            if (string.IsNullOrEmpty(data) || data.Length <= keepLast)
                return new string('*', data.Length);

            return string.Concat(new string('*', data.Length - keepLast), data.AsSpan(data.Length - keepLast));
        }

        [GeneratedRegex(@"<script.*?>.*?</script.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline, "zh-CN")]
        private static partial Regex ScriptRegex();

        [GeneratedRegex(@"<input.*?/>", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex InputRegex();

        [GeneratedRegex(@"<style.*?>.*?</style.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline, "zh-CN")]
        private static partial Regex StyleRegex();

        [GeneratedRegex(@"<iframe.*?>.*?</iframe.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline, "zh-CN")]
        private static partial Regex IframeRegex();

        [GeneratedRegex(@"src[\s]*=[\s]*['""]?javascript:", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex SrcRegex();
    }
}