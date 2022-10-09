using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleCatModels;

/// <summary>
/// 敵がどのステージに出現するか。
/// </summary>
[JsonConverter(typeof(EnemyAppearanceConverter))]
public record EnemyAppearance(int EnemyId, StageRef[] Stages);

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
