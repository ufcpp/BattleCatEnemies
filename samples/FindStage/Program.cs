using BattleCat;
using BattleCat.DataModels;

var finder = await BattleCat.StaticData.Loader.LoadFinder("legend");

//Sample.M(finder); return;

int num;

while (true)
{
    Console.Write("""
    検索したいキャラ数:
    > 
    """);

    if (!int.TryParse(Console.ReadLine(), out var n)) continue;

    num = n;
    break;
}

var selectedEnemies = new Enemy[num];

for (int i = 0; i < num; i++)
{
    Enemy[]? canditates;

    START:
    while (true)
    {
        Console.Write($"""
            {i + 1}体目を検索 (名前で部分一致検索):
            > 
            """);

        var name = Console.ReadLine();
        if (name is null) continue;

        canditates = Enemy.FindByName(finder.AppearedEnemies, name).ToArray();

        if(canditates.Length > 0) break;

        Console.WriteLine("候補が見つかりませんでした");
    }

    while (true)
    {
        Console.WriteLine($"""
            以下の候補から選んでください:
              0. 選びなおし
            """);

        for (int j = 0; j < canditates.Length; j++)
        {
            var e = canditates[j];
            Console.WriteLine($"  {j + 1}. {e.Name}");
        }
        Console.Write("> ");

        var input = Console.ReadLine();
        if (input is null) continue;
        if (!int.TryParse(input, out var selected)) continue;
        if (selected > canditates.Length) continue;

        if (selected == 0) goto START;

        var selectedEnemy = canditates[selected - 1];

        Console.WriteLine($"{selectedEnemy.Name} を選びました");

        selectedEnemies[i] = selectedEnemy;
        break;
    }

}

var enemyIds = selectedEnemies.Select(e => e.Id).ToArray();

Console.WriteLine($"{string.Join(" と ", enemyIds.Select(id => finder.FindEnemy(id)!.Name))} が出てくるステージ(上位5件):");

var stageRefs = finder.FindRaw(enemyIds);

var stages =
    from x in stageRefs
    let stage = finder.Resolve(x)
    orderby stage.Energy()
    select stage;

foreach (var x in stages.Take(5))
{
    Console.WriteLine($"    {x}");
}

class Sample
{
    public static void M(StageFinder finder)
    {
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
            write(finder.FindRaw(enemyIds)!);
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
    }
}
