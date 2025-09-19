using System.Reflection;
using System.Text.RegularExpressions;

namespace Fancyx.Utils
{
    public static class TemplateUtils
    {
        public static string Render(string template, Dictionary<string, object>? variables)
        {
            if (string.IsNullOrWhiteSpace(template)) return template;
            if (variables == null) return template;

            return Regex.Replace(template, @"\{\{(.+?)\}\}", match =>
            {
                string key = match.Groups[1].Value.Trim();
                return GetNestedValue(key, variables)?.ToString() ?? match.Value;
            });
        }

        private static object? GetNestedValue(string path, Dictionary<string, object> variables)
        {
            string[] parts = path.Split('.');
            if (parts.Length == 0 || !variables.TryGetValue(parts[0], out var current))
                return null;

            for (int i = 1; i < parts.Length; i++)
            {
                if (current == null) return null;

                string propertyName = parts[i];
                Type type = current.GetType();

                // 尝试获取属性
                PropertyInfo? property = type.GetProperty(propertyName);
                if (property != null)
                {
                    current = property.GetValue(current);
                    continue;
                }

                // 尝试获取字段
                FieldInfo? field = type.GetField(propertyName);
                if (field != null)
                {
                    current = field.GetValue(current);
                    continue;
                }

                // 尝试字典访问
                if (current is IDictionary<string, object> dict)
                {
                    if (dict.TryGetValue(propertyName, out var value))
                    {
                        current = value;
                        continue;
                    }
                }

                // 如果都不是，返回null
                return null;
            }

            return current;
        }
    }
}