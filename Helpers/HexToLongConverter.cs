using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EdcentralizedNet.Helpers
{
    public class HexToLongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.String)
            {
                var stringVal = reader.GetString();
                
                if(stringVal.StartsWith("0x"))
                {
                    stringVal = stringVal.Remove(0, 2);
                    return long.Parse(stringVal, System.Globalization.NumberStyles.HexNumber);
                }
            }
            else if(reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
