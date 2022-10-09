using System.Text.RegularExpressions;

var content = File.ReadAllText("a.html");

foreach (var (id,name) in scrape(content))
{
    Console.WriteLine($"{id},{name},{Kana.KatakanaToHiragana(name)}");
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

static class Kana
{
    public static string KatakanaToHiragana(string s)
    {
        return string.Create(s.Length, s, static (buffer, s) =>
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = KatakanaToHiragana(s[i]);
            }
        });
    }

    private static char KatakanaToHiragana(char c)
    {
        if (c is >= 'ァ' and <= 'ヶ') return (char)(c - 'ァ' + 'ぁ');
        else return c;
    }
}

static partial class Reg
{
    [GeneratedRegex("""
        <a href="\.\.\/enemy\/(?<id>\d+?)\.html">(?<name>.+?)<\/a>
        """)]
    public static partial Regex Enemy();
}

