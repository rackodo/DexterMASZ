using Newtonsoft.Json;

namespace Launch.Models;

public class UlongConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) =>
        objectType.FullName switch
        {
            "System.UInt64" => true,
            _ => false
        };

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var deserialized = serializer.Deserialize(reader);

        return deserialized is string deStr && ulong.TryParse(deStr, out var value) ? value : deserialized;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
        writer.WriteValue($"{value}");
}
