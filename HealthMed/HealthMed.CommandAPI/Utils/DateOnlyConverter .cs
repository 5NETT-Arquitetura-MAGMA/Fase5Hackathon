using System.Text.Json.Serialization;
using System.Text.Json;

namespace HealthMed.CommandAPI.Utils
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string _serializationFormat = "dd/MM/yyyy";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading DateOnly.");
            }

            if (DateOnly.TryParseExact(reader.GetString(), _serializationFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
            {
                return date;
            }

            throw new JsonException($"Failed to parse DateOnly string '{reader.GetString()}' with format '{_serializationFormat}'.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_serializationFormat, System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}