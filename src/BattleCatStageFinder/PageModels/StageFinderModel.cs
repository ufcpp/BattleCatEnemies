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
    public int Take { get; set; } = 5;

    public Enemy? Enemy1 => Id1 is { } id ? _finder.FindEnemy(id) : null;
    public Enemy? Enemy2 => Id2 is { } id ? _finder.FindEnemy(id) : null;
    public Enemy? Enemy3 => Id3 is { } id ? _finder.FindEnemy(id) : null;

    public void Find()
    {
        var ids = new[] { Id1, Id2, Id3 }
        .Where(id => id != null)
        .Select(id => id.GetValueOrDefault())
        .ToArray();

        Stages = _finder.Find(Take, ids).ToArray();
    }

    public IEnumerable<StageFinder.EntryArray>? Stages { get; private set; }
}
