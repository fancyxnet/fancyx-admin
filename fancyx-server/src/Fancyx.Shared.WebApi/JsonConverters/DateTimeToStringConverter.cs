using Fancyx.Shared.Consts;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fancyx.Shared.WebApi.JsonConverters
{
    public class DateTimeToStringConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return DateTime.MinValue;

            if (reader.TokenType == JsonTokenType.String)
            {
                string? dateString = reader.GetString();
                if (DateTime.TryParseExact(dateString, TimeConsts.DisplayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                throw new JsonException($"Invalid date format. Expected: {TimeConsts.DisplayFormat}, Actual: {dateString}");
            }

            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeConsts.DisplayFormat, CultureInfo.InvariantCulture));
        }
    }

    public class NullableDateTimeToStringConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.String)
            {
                string? dateString = reader.GetString();
                if (DateTime.TryParseExact(dateString, TimeConsts.DisplayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                throw new JsonException($"Invalid date format. Expected: {TimeConsts.DisplayFormat}, Actual: {dateString}");
            }

            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString(TimeConsts.DisplayFormat, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}