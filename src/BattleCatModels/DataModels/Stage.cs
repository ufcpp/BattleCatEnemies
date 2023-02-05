using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCat.DataModels;

/// <summary>
/// 個別のステージ。
/// </summary>
[JsonConverter(typeof(StageConverter))]
public record Stage(string Name, int Energy);

public sealed class StageConverter : JsonConverter<Stage>
{
    public override Stage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        if (!reader.Read() || reader.GetString() is not { } name) throw new JsonException();
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out var energy)) throw new JsonException();
        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) throw new JsonException();

        return new(name, energy);
    }

    public override void Write(Utf8JsonWriter writer, Stage value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.Name);
        writer.WriteNumberValue(value.Energy);
        writer.WriteEndArray();
    }
}
