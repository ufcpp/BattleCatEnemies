using BattleCat;
using BattleCat.DataModels;

namespace BattleCatStageFinder.PageModels;

public class StageFinderModel
{
    private readonly StageFinder _finder;

    public StageFinderModel(StageFinder finder)
    {
        _finder = finder;
    }

    public int? Id1 { get; set; }
    public int? Id2 { get; set; }
    public int? Id3 { get; set; }
    public int Take { get; set; } = 10;

    public void Find()
    {
        var ids = new[] { Id1, Id2, Id3 }
        .Where(id => id != null)
        .Select(id => id.GetValueOrDefault())
        .ToArray();

        var stageRefs = _finder.Find(ids);
        Stages = stageRefs.Select(_finder.Resolve).Take(Take).ToArray();

        Enemies = ids.Select(_finder.FindEnemy)!.ToArray()!;
    }

    public IEnumerable<StageFinder.EntryArray>? Stages { get; private set; }
    public IEnumerable<Enemy>? Enemies { get; private set; }
}
