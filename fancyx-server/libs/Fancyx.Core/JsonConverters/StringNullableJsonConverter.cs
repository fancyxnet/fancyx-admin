//using Fancyx.Core.Utils;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace Fancyx.Core.JsonConverters
//{
//    public class StringNullableJsonConverter : JsonConverter<string?>
//    {
//        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            return StringUtils.XssFilte(reader.GetString());
//        }

//        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
//        {
//            writer.WriteStringValue(value);
//        }
//    }

//    public class StringJsonConverter : JsonConverter<string>
//    {
//        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            return StringUtils.XssFilte(reader.GetString()) ?? "";
//        }

//        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
//        {
//            writer.WriteStringValue(value);
//        }
//    }
//}