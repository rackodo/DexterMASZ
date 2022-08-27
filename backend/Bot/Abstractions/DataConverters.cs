using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Bot.Abstractions;

public class JSONDataConverter<T> : ValueConverter<T, string> where T : ISerializable
{
	public JSONDataConverter() : base(
			v => JsonConvert.SerializeObject(v),
			v => JsonConvert.DeserializeObject<T>(v)
		)
	{
	}
}

public class DictionaryDataConverter<TKey, TValue> : JSONDataConverter<Dictionary<TKey, TValue>>
{
	public DictionaryDataConverter() : base()
	{
	}
}
