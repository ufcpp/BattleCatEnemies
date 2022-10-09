using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCatModels;

/// <summary>
/// 「伝説のはじまり」とか「脆弱性と弱酸性」とかのレベル。
/// </summary>
[JsonConverter(typeof(SectionConverter))]
public record Section(string Name, Stage[] Stages);

public sealed class SectionConverter : JsonConverter<Section>
{
    private static readonly StageConverter _stage = new();

    public override Section Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();
        if (reader.GetString() is not { } name) { Throw(); return null!; }

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();

        var stages = new List<Stage>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            stages.Add(_stage.Read(ref reader, typeof(Stage), options));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) Throw();

        return new(name, stages.ToArray());

        static void Throw() => new JsonException();
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
