using BattleCat;
using BattleCat.DataModels;

var finder = await BattleCat.StaticData.Loader.LoadFinder("legend");

find1(381);
find2(8, 16);
find3(2, 4, 14);
find2(9, 19);
find3(47, 25, 150);
find3(18, 19, 46);
find3(19, 33, 55);

void find1(int enemyId)
{
    var e = finder.FindEnemy(enemyId)!;
    Console.WriteLine($"{e.Name} が出てくるステージ:");
    write(finder.Find(enemyId)!);
}

void find2(int enemyId1, int enemyId2)
{
    var e1 = finder.FindEnemy(enemyId1)!;
    var e2 = finder.FindEnemy(enemyId2)!;
    Console.WriteLine($"{e1.Name} と {e2.Name} が出てくるステージ:");
    write(finder.Find(enemyId1, enemyId2)!);
}

void find3(int enemyId1, int enemyId2, int enemyId3)
{
    var e1 = finder.FindEnemy(enemyId1)!;
    var e2 = finder.FindEnemy(enemyId2)!;
    var e3 = finder.FindEnemy(enemyId3)!;
    Console.WriteLine($"{e1.Name} と {e2.Name} と {e3.Name} が出てくるステージ:");
    write(finder.Find(enemyId1, enemyId2, enemyId3)!);
}

void write(IEnumerable<Array3<StageRef>> stageRefs)
{
    var stages =
        from x in stageRefs
        let stage = finder.Resolve(x)
        orderby stage.Energy()
        select stage;

    foreach (var x in stages.Take(5))
    {
        Console.WriteLine($"    {x}");
    }

    Console.WriteLine();
}
