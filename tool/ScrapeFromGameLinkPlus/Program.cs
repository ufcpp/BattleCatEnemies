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
await File.WriteAllTextAsync("legend.json", JsonSerializer.Serialize(story, options));

static class Converter
{
    private static readonly IReadOnlyDictionary<string, int> _enemyTable;

    static Converter()
    {
        var enemies = BattleCat.StaticData.Loader.LoadEnemies().GetAwaiter().GetResult();
        _enemyTable = enemies.GroupBy(e => e.Name).ToDictionary(g => g.Key, g => g.First().Id);
    }

    public static BattleCat.DataModels.Story Convert(string name, Story x) => new(name, x.Sections.Select(Convert).ToArray(), Convert(x.Enemies));
    public static BattleCat.DataModels.Section Convert(Section x) => new(x.Name, x.Stages.Select(Convert).ToArray());
    public static BattleCat.DataModels.Stage Convert(Stage x) => new(x.Name, x.Stamina);
    public static BattleCat.DataModels.EnemyAppearance[] Convert(Enemy[] x) => x.Select(Convert).ToArray();
    public static BattleCat.DataModels.EnemyAppearance Convert(Enemy x) => new(_enemyTable[x.Name], x.AppearingStages.Select(ToRef).ToArray());
    public static BattleCat.DataModels.StageRef ToRef(Stage x) => new((byte)x.Section.Index, (byte)x.Index);
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
                Console.WriteLine($"    {j++}. {stage.Name} (統率力: {stage.Stamina}) {string.Join(", ", stage.Enemies.Select(e => e.Name))}");
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
