﻿namespace BattleCatModels;

public record Enemy(int Id, string Name, string Kana, string Romaji)
{
    public static Enemy[] LoadFromCsv(string csv)
    {
        var lines = csv.Split('\n');
        var enemies = new Enemy[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var items = line.Split(',');

            if (items.Length < 2) continue;

            var id = int.Parse(items[0]);
            var name = items[1];
            var kana = items[2];

            // kana is empty if it can be obtained by a simple transformation from name.
            if(string.IsNullOrEmpty(kana)) kana = Japanese.Kana.KatakanaToHiragana(name);

            enemies[i] = new(id, name, kana, Japanese.Kana.HirakanaToRomaji(kana));
        }

        return enemies;
    }

    public static IEnumerable<Enemy> FindByName(Enemy[] enemies, string name)
    {
        var list = new List<Enemy>();
        var kname = Japanese.Kana.KatakanaToHiragana(name);
        var lname = name.ToLower();

        foreach (var enemy in enemies)
        {
            if (enemy.Name.Contains(name)
                || enemy.Kana.Contains(kname)
                || enemy.Romaji.Contains(lname)
                ) list.Add(enemy);
        }

        return list;
    }
}