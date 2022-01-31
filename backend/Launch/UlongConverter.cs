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
		return ulong.Parse(new((existingValue as string).Where(char.IsDigit).ToArray()));
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		writer.WriteValue($"{value}");
	}
}
