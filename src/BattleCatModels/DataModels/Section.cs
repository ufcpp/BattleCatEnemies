using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCat.DataModels;

/// <summary>
/// 「伝説のはじまり」とか「脆弱性と弱酸性」とかのレベル。
/// </summary>
[JsonConverter(typeof(SectionConverter))]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public record Section(string Name, Stage[] Stages)
{
    private string GetDebuggerDisplay() => $"{Name}: {string.Join(", ", Stages.AsEnumerable())}";
}

public sealed class SectionConverter : JsonConverter<Section>
{
    private static readonly StageConverter _stage = new();

    public override Section Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        if (!reader.Read() || reader.GetString() is not { } name) throw new JsonException();

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) throw new JsonException();

        var stages = new List<Stage>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            stages.Add(_stage.Read(ref reader, typeof(Stage), options));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) throw new JsonException();

        return new(name, stages.ToArray());
    }

    public override void Write(Utf8JsonWriter writer, Section value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.Name);

        writer.WriteStartArray();
        foreach (var stage in value.Stages) _stage.Write(writer, stage, options);
        writer.WriteEndArray();

        writer.WriteEndArray();
    }
}
