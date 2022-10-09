using BattleCatModels;

var enemies = Enemy.LoadFromCsv(await File.ReadAllTextAsync("enemies.txt"));

while (true)
{
    var line = Console.ReadLine();

    if (string.IsNullOrEmpty(line)) break;

    var found = Enemy.FindByName(enemies, line);

    foreach (var e in found)
    {
        Console.WriteLine(e);
    }
}
