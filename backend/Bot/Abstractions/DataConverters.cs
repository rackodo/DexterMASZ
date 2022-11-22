using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Bot.Abstractions;

public class JsonDataConverter<T> : ValueConverter<T, string> where T : ISerializable
{
    public JsonDataConverter() : base(
        v => JsonConvert.SerializeObject(v),
        v => JsonConvert.DeserializeObject<T>(v)
    )
    {
    }
}

public class DictionaryDataConverter<TKey, TValue> : JsonDataConverter<Dictionary<TKey, TValue>>
{
}
