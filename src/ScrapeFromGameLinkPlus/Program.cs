using ScrapeFromGameLinkPlus;
using System.Text.Encodings.Web;
using System.Text.Json;

var url = "https://gamelinkplus.com/battlecats/legend-stage-enemy-list/";

var content = await Loader.LoadAsync(url);
var data = Parser.Parse(content);

//Display.ShowSections(data.Sections);
//Display.ShowEnemies(data.Enemies);

var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    //WriteIndented = true
};

var story = Converter.Convert("レジェンド", data);
await File.WriteAllTextAsync("legend_stage.json", JsonSerializer.Serialize(story, options));

var enemies = Converter.Convert(data.Enemies);
await File.WriteAllTextAsync("legend_enemies.json", JsonSerializer.Serialize(enemies, options));

static class Converter
{
    private static IReadOnlyDictionary<string, int> _enemyTable;

    static Converter()
    {
        var enemies = BattleCatModels.Enemy.LoadFromCsv(File.ReadAllText("enemies.txt"));
        _enemyTable = enemies.GroupBy(e => e.Name).ToDictionary(g => g.Key, g => g.First().Id);
    }

    public static BattleCatModels.Story Convert(string name, Story x) => new(name, x.Sections.Select(Convert).ToArray());
    public static BattleCatModels.Section Convert(Section x) => new(x.Name, x.Stages.Select(Convert).ToArray());
    public static BattleCatModels.Stage Convert(Stage x) => new(x.Name, x.Energy);
    public static BattleCatModels.EnemyAppearance[] Convert(Enemy[] x) => x.Select(Convert).ToArray();
    public static BattleCatModels.EnemyAppearance Convert(Enemy x) => new(_enemyTable[x.Name], x.AppearingStages.Select(ToRef).ToArray());
    public static BattleCatModels.StageRef ToRef(Stage x) => new(x.Section.Index, x.Index);
}

static class Loader
{
    public static async ValueTask<string> LoadAsync(string url)
    {
        var c = new HttpClient();
        var res = await c.GetAsync(url);
        return await res.Content.ReadAsStringAsync();
    }
}

static class Display
{
    public static void ShowSections(Section[] sections)
    {
        var i = 1;
        foreach (var sec in sections)
        {
            Console.WriteLine($"{i++}. {sec.Name}");

            var j = 1;
            foreach (var stage in sec.Stages)
            {
                Console.WriteLine($"    {j++}. {stage.Name} (統率力: {stage.Energy}) {string.Join(", ", stage.Enemies.Select(e => e.Name))}");
            }
        }
    }

    public static void ShowEnemies(Enemy[] enemies)
    {
        foreach (var e in enemies)
        {
            Console.WriteLine($"{e.Name} (登場回数: {e.AppearingStages.Count})");
        }
    }
}
