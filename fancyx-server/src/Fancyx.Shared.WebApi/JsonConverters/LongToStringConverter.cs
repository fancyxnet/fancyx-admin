using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fancyx.Shared.WebApi.JsonConverters
{
    public class LongToStringConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return 0L;

            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString()!;
                if (long.TryParse(stringValue, out long result))
                {
                    return result;
                }
                throw new JsonException($"Invalid long format: {stringValue}");
            }

            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public class NullableLongToStringConverter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString()!;
                if (long.TryParse(stringValue, out long result))
                {
                    return result;
                }
                throw new JsonException($"Invalid long format: {stringValue}");
            }

            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString());
            else
                writer.WriteNullValue();
        }
    }
}