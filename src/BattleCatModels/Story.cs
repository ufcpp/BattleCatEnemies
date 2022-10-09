using System.Text.Json.Serialization;
using System.Text.Json;

namespace BattleCatModels;

/// <summary>
/// 「レジェンド」とか「真レジェンド」とかのレベル。
/// </summary>
[JsonConverter(typeof(StoryConverter))]
public record Story(string Name, Section[] Sections, EnemyAppearance[] Enemies);

public sealed class StoryConverter : JsonConverter<Story>
{
    private static readonly SectionConverter _section = new();
    private static readonly EnemyAppearanceConverter _enemy = new();

    public override Story Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        if (!reader.Read() || reader.GetString() is not { } name) throw new JsonException();

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        var sections = new List<Section>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            sections.Add(_section.Read(ref reader, typeof(Section), options));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        var enemies = new List<EnemyAppearance>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            enemies.Add(_enemy.Read(ref reader, typeof(EnemyAppearance), options));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) throw new JsonException();

        return new(name, sections.ToArray(), enemies.ToArray());
    }

    public override void Write(Utf8JsonWriter writer, Story value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.Name);

        writer.WriteStartArray();
        foreach (var stage in value.Sections) _section.Write(writer, stage, options);
        writer.WriteEndArray();

        writer.WriteStartArray();
        foreach (var enemy in value.Enemies) _enemy.Write(writer, enemy, options);
        writer.WriteEndArray();

        writer.WriteEndArray();
    }
}
