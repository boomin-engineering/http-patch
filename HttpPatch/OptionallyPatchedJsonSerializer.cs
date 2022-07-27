using System.Diagnostics.CodeAnalysis;

namespace HttpPatch;

[SuppressMessage("", "CA1034", Justification = "Nicer to group the 2 serializers together")]
internal static class OptionallyPatchedJsonSerializer
{
    [SuppressMessage("", "CA1812", Justification = "Used in attributes")]
    internal class NewtonsoftJsonConverter : Newtonsoft.Json.JsonConverter<IOptionallyPatched>
    {
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, IOptionallyPatched? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is { IsIncludedInPatch: true })
            {
                serializer.Serialize(writer, value.Value);
            }
            else
            {
                writer.WriteUndefined();
            }
        }

        public override IOptionallyPatched? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, IOptionallyPatched? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var underlyingType = objectType.GetGenericArguments().Single();

            if (reader.TokenType == Newtonsoft.Json.JsonToken.Undefined)
            {
                return (IOptionallyPatched)Activator.CreateInstance(objectType, false, default)!;
            }

            var deserializedItem = serializer.Deserialize(reader, underlyingType);
            return (IOptionallyPatched)Activator.CreateInstance(objectType, true, deserializedItem)!;
        }
    }

    [SuppressMessage("", "CA1812", Justification = "Used in attributes")]
    internal class SystemTextJson<T> : System.Text.Json.Serialization.JsonConverter<OptionallyPatched<T>>
    {
        public override void Write(System.Text.Json.Utf8JsonWriter writer, OptionallyPatched<T> value, System.Text.Json.JsonSerializerOptions options)
            => throw new NotImplementedException();

        public override OptionallyPatched<T> Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var underlyingType = typeToConvert.GetGenericArguments().Single();
            var deserializedItem = System.Text.Json.JsonSerializer.Deserialize(ref reader, underlyingType, options);
            var optionallyPatchedWrapper = Activator.CreateInstance(typeToConvert, new[] { true, deserializedItem });
            return (OptionallyPatched<T>)optionallyPatchedWrapper!;
        }
    }

    [SuppressMessage("", "CA1812", Justification = "Used in attributes")]
    internal class SystemTextJsonFactory : System.Text.Json.Serialization.JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(OptionallyPatched<>);

        public override System.Text.Json.Serialization.JsonConverter? CreateConverter(Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var underlyingType = typeToConvert.GetGenericArguments().Single();
            var genericType = typeof(SystemTextJson<>).MakeGenericType(underlyingType);
            return (System.Text.Json.Serialization.JsonConverter)Activator.CreateInstance(genericType)!;
        }
    }
}