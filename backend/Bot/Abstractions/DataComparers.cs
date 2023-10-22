using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Bot.Abstractions;

public class JsonDataComparer<T> : ValueComparer<T>
{
    public JsonDataComparer() : base(
        (v1, v2) => JsonConvert.SerializeObject(v1) == JsonConvert.SerializeObject(v2),
        v => v.GetHashCode(),
        v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))
    )
    {
    }
}

public class ListDataComparer<T> : JsonDataComparer<List<T>>
{

}

public class DictionaryDataComparer<TKey, TValue> : ValueComparer<Dictionary<TKey, TValue>>
{
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
        if (d2.Keys.Except(d1.Keys).Any()) return false;

        if (d1.Count != d2.Count)
            return false;

        foreach (var kvp in d2)
        {
            if (!kvp.Value.Equals(d1[kvp.Key]))
                return false;
        }

        foreach (var kvp in d1)
        {
            if (!kvp.Value.Equals(d2[kvp.Key]))
                return false;
        }

        return true;
    }
}
