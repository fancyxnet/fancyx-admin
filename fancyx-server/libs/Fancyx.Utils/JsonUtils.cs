using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fancyx.Utils
{
    public static class JsonUtils
    {
        // 配置全局 JSON 序列化设置：驼峰命名 + 其他常用选项
        private static readonly JsonSerializerSettings _settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Formatting.None // 紧凑格式，节省带宽
        };

        /// <summary>
        /// 将对象序列化为 JSON 字符串（使用驼峰命名）
        /// </summary>
        public static string Serialize(object obj)
        {
            if (obj == null) return "null";
            return JsonConvert.SerializeObject(obj, _settings);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为指定类型（支持驼峰命名映射到 C# PascalCase 属性）
        /// </summary>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) return default!;
            return JsonConvert.DeserializeObject<T>(json, _settings)!;
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为 object（动态解析）
        /// </summary>
        public static object? DeserializeObject(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            return JsonConvert.DeserializeObject(json, _settings);
        }
    }
}
