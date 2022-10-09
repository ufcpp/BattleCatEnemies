using System.Text.Json;

namespace BattleCatModels.Data;

/// <summary>
/// Copy to Output Directory でビルド時にコピーしてるデータからの読み込み。
/// </summary>
public class Loader
{
    public static async ValueTask<Enemy[]> LoadEnemies()
    {
        var file = await File.ReadAllTextAsync("Data/enemies.txt");
        var enemies = Enemy.LoadFromCsv(file);
        return enemies;
    }

    public static async ValueTask<Story> LoadStory(string stageName)
    {
        var file = await File.ReadAllTextAsync($"Data/Stages/{stageName}.json");
        var story = JsonSerializer.Deserialize<Story>(file)!;
        return story;
    }

    public static IEnumerable<string> EnumerateStages()
        => Directory.EnumerateFiles("Data/Stages")
        .Select(x => Path.GetFileNameWithoutExtension(x));
}
