using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCat.DataModels;

/// <summary>
/// インデックスだけあれば「何 <see name="Section"/> 目の何 <see cref="Stage"/>」が特定できるので int で参照。
/// </summary>
[JsonConverter(typeof(StageRefConverter))]
public record struct StageRef(byte Section, byte Stage) : IComparable<StageRef>
{
    public int CompareTo(StageRef other)
    {
        if (Section.CompareTo(other.Section) is var x && x != 0) return x;
        return Stage.CompareTo(other.Stage);
    }
}

public sealed class StageRefConverter : JsonConverter<StageRef>
{
    public override StageRef Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out var section)) throw new JsonException();
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out var stage)) throw new JsonException();
        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) throw new JsonException();

        return new((byte)section, (byte)stage);
    }

    public override void Write(Utf8JsonWriter writer, StageRef value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Section);
        writer.WriteNumberValue(value.Stage);
        writer.WriteEndArray();
    }
}
