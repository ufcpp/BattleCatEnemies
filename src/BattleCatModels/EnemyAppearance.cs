using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCatModels;

/// <summary>
/// 敵がどのステージに出現するか。
/// </summary>
[JsonConverter(typeof(EnemyAppearanceConverter))]
public record EnemyAppearance(int EnemyId, StageRef[] Stages);

/// <summary>
/// インデックスだけあれば「何 <see name="Section"/> 目の何 <see cref="Stage"/>」が特定できるので int で参照。
/// </summary>
[JsonConverter(typeof(StageRefConverter))]
public record struct StageRef(int Section, int Stage);

public sealed class EnemyAppearanceConverter : JsonConverter<EnemyAppearance>
{
    private static readonly StageRefConverter _stageRef = new();

    public override EnemyAppearance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();
        if (!reader.TryGetInt32(out var enemyId)) Throw();

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) Throw();

        var stages = new List<StageRef>();
        while(reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            stages.Add(_stageRef.Read(ref reader, typeof(StageRef), options));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) Throw();

        return new(enemyId, stages.ToArray());

        static void Throw() => new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, EnemyAppearance value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.EnemyId);

        writer.WriteStartArray();
        foreach (var stage in value.Stages) _stageRef.Write(writer, stage, options);
        writer.WriteEndArray();

        writer.WriteEndArray();
    }
}

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
