using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCat.DataModels;

/// <summary>
/// 敵がどのステージに出現するか。
/// </summary>
[JsonConverter(typeof(EnemyAppearanceConverter))]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public record EnemyAppearance(int EnemyId, StageRef[] Stages)
{
    private string GetDebuggerDisplay() => $"{EnemyId}: {string.Join(", ", Stages.Select(x => x.GetDebuggerDisplay()))}";
}

public sealed class EnemyAppearanceConverter : JsonConverter<EnemyAppearance>
{
    private static readonly StageRefConverter _stageRef = new();

    public override EnemyAppearance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out var enemyId)) throw new JsonException();

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
        var stages = new List<StageRef>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            stages.Add(_stageRef.Read(ref reader, typeof(StageRef), options));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) throw new JsonException();

        return new(enemyId, stages.ToArray());
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
