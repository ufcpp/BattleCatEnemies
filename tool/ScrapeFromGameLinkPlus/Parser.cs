using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ScrapeFromGameLinkPlus;

static partial class HtmlTag
{
    [GeneratedRegex("""\<span.*?\>(?<value>.+?)\</span""")]
    public static partial Regex Span();

    [GeneratedRegex("""\<td\>(?<value>.+?)\</td""")]
    public static partial Regex Td();

    [GeneratedRegex("""\<a.*?\>(?<value>.+?)\</a\>""")]
    public static partial Regex A();

    public const char Splitter = '、';

    public static readonly string SplitterStr = $"{Splitter}";
}

static class Parser
{
    public static Story Parse(string content)
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
                .Where(t => t.Contains("ステージ名"))
                .Select(t => t.Replace("\n", ""));

            var i = 0;
            foreach (var x in items)
            {
                var m = HtmlTag.Span().Match(x);
                System.Diagnostics.Debug.Assert(m.Success);

                var name = m.Groups["value"].Value;
                var section = new Section(name, i++, ParseStage(x, enemies));
                foreach (var stage in section.Stages) stage.Section = section;
                yield return section;
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

            var i = 0;
            foreach (var x in items)
            {
                var m = HtmlTag.Td().Match(x);
                System.Diagnostics.Debug.Assert(m.Success);

                var name = m.Groups["value"].Value;

                m = m.NextMatch();
                System.Diagnostics.Debug.Assert(m.Success);

                var stamina = int.Parse(m.Groups["value"].Value);

                m = m.NextMatch();
                System.Diagnostics.Debug.Assert(m.Success);

                var enemiesLineHtml = m.Groups["value"].Value;
                var enemiesLine = HtmlTag.A()
                    .Replace(enemiesLineHtml, m => m.Groups["value"].Value)
                    .Replace("<br />", HtmlTag.SplitterStr);

                var stage = new Stage(name, i++, stamina, ParseEmeny(enemiesLine, enemies));
                foreach (var e in stage.Enemies) e.AppearingStages.Add(stage);
                yield return stage;
            }
        }
    }

    public static Enemy[] ParseEmeny(string content, Dictionary<string, Enemy> enemies)
    {
        return inner().ToArray();

        IEnumerable<Enemy> inner()
        {
            foreach (var name in content.Split(HtmlTag.Splitter))
            {
                var enemy = getEnemy(enemies, name.Trim());
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
