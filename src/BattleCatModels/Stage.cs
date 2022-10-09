using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCatModels;

/// <summary>
/// 個別のステージ。
/// </summary>
[JsonConverter(typeof(StageConverter))]
public record Stage(string Name, int Energy);

public sealed class StageConverter : JsonConverter<Stage>
{
    public override Stage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();
        if (reader.GetString() is not { } name) { Throw(); return null!; }
        if (!reader.TryGetInt32(out var energy)) Throw();
        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) Throw();

        return new(name, energy);

        static void Throw() => new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Stage value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.Name);
        writer.WriteNumberValue(value.Energy);
        writer.WriteEndArray();
    }
}
