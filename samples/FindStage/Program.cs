using BattleCat;
using BattleCat.DataModels;

var finder = await BattleCat.StaticData.Loader.LoadFinder("legend");

find(381);
find(8, 16);
find(2, 4, 14);
find(9, 19);
find(47, 25, 150);
find(18, 19, 46);
find(19, 33, 55);

void find(params int[] enemyIds)
{
    Console.WriteLine($"{string.Join(" と ", enemyIds.Select(id => finder.FindEnemy(id)!.Name))} が出てくるステージ:");
    write(finder.Find(enemyIds)!);
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
