using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpPatch.Schema;

internal class EnumMemberJsonConverter<T> : JsonConverter<T>
    where T : Enum
{
    private static readonly IDictionary<T, string> ValueMap = Enum
        .GetValues(typeof(T))
        .Cast<T>()
        .ToDictionary(v => v, v => typeof(T).GetField(v.ToString())?.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? v.ToString());

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        return ValueMap.FirstOrDefault(kv => string.Equals(kv.Value, str, StringComparison.InvariantCultureIgnoreCase)).Key;
    }
        
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(ValueMap[value]);
    }
}