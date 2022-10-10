using BattleCat.DataModels;

var enemies = await BattleCat.StaticData.Loader.LoadEnemies();

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
