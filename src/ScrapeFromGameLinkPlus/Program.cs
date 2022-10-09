using ScrapeFromGameLinkPlus;

var url = "https://gamelinkplus.com/battlecats/legend-stage-enemy-list/";

var content = await Loader.LoadAsync(url);
var data = Parser.Parse(content);

//Display.ShowSections(data.Sections);
Display.ShowEnemies(data.Enemies);


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
                Console.WriteLine($"    {j++}. {stage.Name} (“—¦—Í: {stage.Stamina}) {string.Join(", ", stage.Enemies.Select(e => e.Name))}");
            }
        }
    }

    public static void ShowEnemies(Enemy[] enemies)
    {
        foreach (var e in enemies)
        {
            Console.WriteLine($"{e.Name} (“oê‰ñ”: {e.AppearingStages.Count})");
        }
    }

}
