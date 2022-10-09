using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ScrapeFromGameLinkPlus;

static partial class HtmlTag
{
    [GeneratedRegex("""\<span.*?\>(?<value>.+?)\<""")]
    public static partial Regex Span();

    [GeneratedRegex("""\<td\>(?<value>.+?)\<""", RegexOptions.Multiline)]
    public static partial Regex Td();

    [GeneratedRegex("""\<a.*?\>(?<value>.+?)\<""")]
    public static partial Regex A();
}

static class Parser
{
    public static NyankoData Parse(string content)
    {
        Dictionary<string, Enemy> enemies = new();
        var sections = ParseSection(content, enemies);
        return new(sections, enemies.Values.ToArray());
    }

    public static Section[] ParseSection(string content, Dictionary<string, Enemy> enemies)
    {
        return inner().ToArray();

        IEnumerable<Section> inner()
        {
            var items = content.Split("<h2>")
                .Where(t => t.Contains("ステージ名"));

            foreach (var x in items)
            {
                var m = HtmlTag.Span().Match(x);
                System.Diagnostics.Debug.Assert(m.Success);

                var name = m.Groups["value"].Value;
                yield return new(name, ParseStage(x, enemies));
            }
        }
    }

    public static Stage[] ParseStage(string content, Dictionary<string, Enemy> enemies)
    {
        return inner().ToArray();

        IEnumerable<Stage> inner()
        {
            var items = content.Split("<tr>")
                .Where(t => t.Contains("<td>"));

            foreach (var x in items)
            {
                var m = HtmlTag.Td().Match(x);
                System.Diagnostics.Debug.Assert(m.Success);

                var name = m.Groups["value"].Value;

                m = m.NextMatch();
                System.Diagnostics.Debug.Assert(m.Success);

                var stamina = int.Parse(m.Groups["value"].Value);

                var stage = new Stage(name, stamina, ParseEmeny(x, enemies));

                foreach (var e in stage.Enemies)
                {
                    e.AppearingStages.Add(stage);
                }

                yield return stage;
            }
        }
    }

    public static Enemy[] ParseEmeny(string content, Dictionary<string, Enemy> enemies)
    {
        return inner().ToArray();

        IEnumerable<Enemy> inner()
        {
            for (var m = HtmlTag.A().Match(content); m.Success; m = m.NextMatch())
            {
                var name = m.Groups["value"].Value;
                if (name.Contains("の情報は")) continue;
                var enemy = getEnemy(enemies, name);
                yield return enemy;
            }
        }

        static Enemy getEnemy(Dictionary<string, Enemy> enemies, string name)
        {
            ref var enemy = ref CollectionsMarshal.GetValueRefOrAddDefault(enemies, name, out _);
            enemy ??= new(name);
            return enemy;
        }
    }
}
