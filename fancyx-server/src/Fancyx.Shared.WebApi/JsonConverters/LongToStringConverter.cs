using Newtonsoft.Json;

namespace Fancyx.Shared.WebApi.JsonConverters
{
    public class LongToStringConverter : JsonConverter<long>
    {
        public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return 0L;

            if (reader.TokenType == JsonToken.String)
            {
                string? stringValue = reader.Value?.ToString();
                if (long.TryParse(stringValue, out long result))
                {
                    return result;
                }
                throw new JsonSerializationException($"Invalid long format: {stringValue}");
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }

    public class NullableLongToStringConverter : JsonConverter<long?>
    {
        public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                string? stringValue = reader.Value?.ToString();
                if (long.TryParse(stringValue, out long result))
                {
                    return result;
                }
                throw new JsonSerializationException($"Invalid long format: {stringValue}");
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value.ToString());
            else
                writer.WriteNull();
        }
    }
}