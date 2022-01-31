using Newtonsoft.Json;

namespace Launch;

public class UlongConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType.FullName switch
		{
			"System.UInt64" => true,
			_ => false,
		};
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		var deserialized = serializer.Deserialize(reader);

		if (deserialized is string deStr)
			if (ulong.TryParse(deStr, out var value))
				return value;

		return deserialized;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		writer.WriteValue($"{value}");
	}
}
