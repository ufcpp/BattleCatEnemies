using System.Text.RegularExpressions;

var content = File.ReadAllText("a.html");

foreach (var (id,name) in scrape(content))
{
    Console.WriteLine($"{id},{name},");
}

static IEnumerable<(int id, string name)> scrape(string content)
{
    for (var m = Reg.Enemy().Match(content); m.Success; m = m.NextMatch())
    {
        var id = int.Parse(m.Groups["id"].Value);
        var name = m.Groups["name"].Value;
        yield return (id, name);
    }
}

static partial class Reg
{
    [GeneratedRegex("""
        <a href="\.\.\/enemy\/(?<id>\d+?)\.html">(?<name>\w+?)<\/a>
        """)]
    public static partial Regex Enemy();
}

