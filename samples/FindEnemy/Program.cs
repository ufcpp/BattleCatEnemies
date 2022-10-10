using BattleCatModels;

var enemies = await BattleCatModels.Data.Loader.LoadEnemies();

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
