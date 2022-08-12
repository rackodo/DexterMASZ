using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Bot.Abstractions;

public class JSONDataComparer<T> : ValueComparer<T> where T : ISerializable
{
	public JSONDataComparer() : base(
			(v1, v2) => JsonConvert.SerializeObject(v1) == JsonConvert.SerializeObject(v2),
			v => v.GetHashCode(),
			v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))
		)
	{
	}
}

public class DictionaryDataComparer<TKey, TValue> : ValueComparer<Dictionary<TKey, TValue>> {

	public DictionaryDataComparer() : base(
		(v1, v2) => AreDictionariesEqual(v1, v2),
		v => v.GetHashCode(),
		v => v.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
		)
    {
    }

	private static bool AreDictionariesEqual(Dictionary<TKey, TValue> d1, Dictionary<TKey, TValue> d2)
    {
		if (d1.Keys.Except(d2.Keys).Any()) return false;

		foreach (var kvp in d1)
		{
			if (!kvp.Value.Equals(d1[kvp.Key]))
				return false;

		}

		return true;
    }
}
