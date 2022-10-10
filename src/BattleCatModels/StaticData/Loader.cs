using BattleCat.DataModels;
using System.Text.Json;

namespace BattleCat.StaticData;

/// <summary>
/// Copy to Output Directory でビルド時にコピーしてるデータからの読み込み。
/// </summary>
public class Loader
{
    public static async ValueTask<Enemy[]> LoadEnemies()
    {
        var file = await File.ReadAllTextAsync("StaticData/enemies.txt");
        var enemies = Enemy.LoadFromCsv(file);
        return enemies;
    }

    public static async ValueTask<Story> LoadStory(string stageName)
    {
        var file = await File.ReadAllTextAsync($"StaticData/Stages/{stageName}.json");
        var story = JsonSerializer.Deserialize<Story>(file)!;
        return story;
    }

    public static IEnumerable<string> EnumerateStages()
        => Directory.EnumerateFiles("StaticData/Stages")
        .Select(x => Path.GetFileNameWithoutExtension(x));
}
