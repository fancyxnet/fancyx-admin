using Fancyx.Shared.Consts;

using Newtonsoft.Json;

namespace Fancyx.Shared.WebApi.JsonConverters
{
    public class DateTimeToStringConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return DateTime.MinValue;

            if (reader.TokenType == JsonToken.String)
            {
                string? dateString = reader.Value?.ToString();
                if (DateTime.TryParseExact(dateString, TimeConsts.DisplayFormat, null, System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                throw new JsonSerializationException($"Invalid date format. Expected: {TimeConsts.DisplayFormat}, Actual: {dateString}");
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(TimeConsts.DisplayFormat));
        }
    }

    public class NullableDateTimeToStringConverter : JsonConverter<DateTime?>
    {
        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                string? dateString = reader.Value?.ToString();
                if (DateTime.TryParseExact(dateString, TimeConsts.DisplayFormat, null, System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                throw new JsonSerializationException($"Invalid date format. Expected: {TimeConsts.DisplayFormat}, Actual: {dateString}");
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value.ToString(TimeConsts.DisplayFormat));
            else
                writer.WriteNull();
        }
    }
}