using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCatModels;

/// <summary>
/// インデックスだけあれば「何 <see name="Section"/> 目の何 <see cref="Stage"/>」が特定できるので int で参照。
/// </summary>
[JsonConverter(typeof(StageRefConverter))]
public record struct StageRef(int Section, int Stage);

public sealed class StageRefConverter : JsonConverter<StageRef>
{
    public override StageRef Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();
        if (!reader.TryGetInt32(out var section)) Throw();
        if (!reader.TryGetInt32(out var stage)) Throw();
        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) Throw();

        return new(section, stage);

        static void Throw() => new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, StageRef value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Section);
        writer.WriteNumberValue(value.Stage);
        writer.WriteEndArray();
    }
}
