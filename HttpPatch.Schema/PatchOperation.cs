using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpPatch.Schema;

public class PatchOperation
{
    [JsonPropertyName("op")]
    [JsonConverter(typeof(EnumMemberJsonConverter<PatchOperationType>))]
    [Required]
    public PatchOperationType Type { get; set; }

    [Required]
    public string Path { get; set; } = null!;

    public object? Value { get; set; }

    public T? ValueAs<T>(T? defaultValue = default)
    {
        if (Value == null)
        {
            return defaultValue;
        }

        if (Value is not JsonElement valueElement)
        {
            throw new JsonException("Value is not an JsonElement.");
        }

        if (typeof(T).IsEnum)
        {
            var value = valueElement.GetString();
                
            if (!Enum.TryParse(typeof(T), value, true, out var enumValue))
            {
                throw new PatchOperationValidationException($"'{value}' is not a valid value for {typeof(T).Name}");
            }
            return (T?)enumValue;
        }
            
        var bufferWriter = new ArrayBufferWriter<byte>(1024);
        using var writer = new Utf8JsonWriter(bufferWriter);
        valueElement.WriteTo(writer);
        writer.Flush();
            
        return JsonSerializer.Deserialize<T?>(bufferWriter.WrittenSpan, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
    }
}