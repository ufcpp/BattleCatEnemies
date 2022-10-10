using BattleCat;
using BattleCat.DataModels;

namespace BattleCatStageFinder.PageModels;

public class StageFinderModel
{
    private readonly StageFinder _finder;

    public StageFinderModel(StageFinder finder)
    {
        _finder = finder;
        var enemies = finder.AppearedEnemies.ToArray();
        Options1 = new(enemies);
        Options2 = new(enemies);
        Options3 = new(enemies);
    }

    public int Take { get; set; } = 5;

    public void Find()
    {
        var ids = new[] { Options1.SelectedEnemyId, Options2.SelectedEnemyId, Options3.SelectedEnemyId }
        .Where(id => id != null)
        .Select(id => id.GetValueOrDefault())
        .ToArray();

        Stages = _finder.Find(Take, ids).ToArray();
    }

    public IEnumerable<StageFinder.EntryArray>? Stages { get; private set; }

    public EnemyNameOptions Options1 { get; }
    public EnemyNameOptions Options2 { get; }
    public EnemyNameOptions Options3 { get; }
}

public class EnemyNameOptions
{
    private readonly EnemyNameOption[] _options;

    public string? SelectedValue { get; set; }

    public string? Filter { get => _filter; set => ChangeFilter(value); }
    private string? _filter;

    private void ChangeFilter(string? filter)
    {
        _filter = filter;
        foreach (var x in _options) x.Filter(filter);
    }

    public EnemyNameOptions(Enemy[] enemies)
    {
        _options = enemies.Select(e => new EnemyNameOption(e)).ToArray();
    }

    public IEnumerable<EnemyNameOption> Options => _options;

    public int? SelectedEnemyId => _options.FirstOrDefault(x => x.Name == SelectedValue)?.Id;
}

public class EnemyNameOption
{
    private Enemy _enemy;
    public bool Disabled { get; private set; }

    public EnemyNameOption(Enemy enemy)
    {
        _enemy = enemy;
    }

    internal void Filter(string? filter)
    {
        Disabled = filter is { } f && !_enemy.Match(f);
    }

    public string Name => _enemy.Name;
    public int Id => _enemy.Id;
}
