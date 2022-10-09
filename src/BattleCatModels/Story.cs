using System.Text.Json.Serialization;
using System.Text.Json;

namespace BattleCatModels;

/// <summary>
/// 「レジェンド」とか「真レジェンド」とかのレベル。
/// </summary>
[JsonConverter(typeof(StoryConverter))]
public record Story(string Name, Section[] Sections, EnemyAppearance[] Enemies);

/// <summary>
/// 「伝説のはじまり」とか「脆弱性と弱酸性」とかのレベル。
/// </summary>
[JsonConverter(typeof(SectionConverter))]
public record Section(string Name, Stage[] Stages);

/// <summary>
/// 個別のステージ。
/// </summary>
[JsonConverter(typeof(StageConverter))]
public record Stage(string Name, int Energy);

public sealed class StoryConverter : JsonConverter<Story>
{
    private static readonly SectionConverter _section = new();
    private static readonly EnemyAppearanceConverter _enemy = new();

    public override Story Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();
        if (reader.GetString() is not { } name) { Throw(); return null!; }

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();

        var sections = new List<Section>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            sections.Add(_section.Read(ref reader, typeof(Section), options));
        }

        var enemies = new List<EnemyAppearance>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            enemies.Add(_enemy.Read(ref reader, typeof(EnemyAppearance), options));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) Throw();

        return new(name, sections.ToArray(), enemies.ToArray());

        static void Throw() => new JsonException();
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
