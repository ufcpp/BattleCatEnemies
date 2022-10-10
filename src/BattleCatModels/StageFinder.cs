using BattleCat.DataModels;

namespace BattleCat;

public class StageFinder
{
    public Story Story { get; }
    private readonly Enemy[] _appearedEnemies;
    private readonly Dictionary<int, EnemyAppearance> _enemyMap;

    public record struct Entry(Section Section, Stage Stage)
    {
        public int Energy() => Stage.Energy;
        public override string ToString() => $"{Section.Name} - {Stage.Name} ({Stage.Energy})";
    }

    public StageFinder(Story story, Enemy[] enemies)
    {
        Story = story;

        var map = enemies.ToDictionary(e => e.Id);

        _appearedEnemies =
            story.Enemies
            .Select(e => map[e.EnemyId])
            .ToArray();

        _enemyMap = story.Enemies.ToDictionary(e => e.EnemyId);
    }

    public Enemy? FindEnemy(int enemyId) => _appearedEnemies.FirstOrDefault(e => e.Id == enemyId);

    /// <summary>
    /// 敵が出るステージを検索。
    /// </summary>
    public IEnumerable<StageRef>? Find(int enemyId)
        => _enemyMap.TryGetValue(enemyId, out var e) ? e.Stages : null;

    /// <summary>
    /// 2種の敵が同時に出るステージを検索。
    /// </summary>
    public IEnumerable<StageRef>? Find(int enemyId1, int enemyId2)
    {
        if (Find(enemyId1) is not { } stages1) return null;
        if (Find(enemyId2) is not { } stages2) return null;
        return stages1.Intersect(stages2);
    }

    /// <summary>
    /// 3種の敵が同時に出るステージを検索。
    /// </summary>
    /// <remarks>
    /// 最大3種まで同時にミッションに現れるので、3引数まで用意。
    /// </remarks>
    public IEnumerable<StageRef>? Find(int enemyId1, int enemyId2, int enemyId3)
    {
        if (Find(enemyId1, enemyId2) is not { } stages1) return null;
        if (Find(enemyId3) is not { } stages2) return null;
        return stages1.Intersect(stages2);
    }

    public Entry Resolve(StageRef stageRef)
    {
        var section = Story.Sections[stageRef.Section];
        var stage = section.Stages[stageRef.Stage];
        return new(section, stage);
    }

    /// <summary>
    /// ストーリー中に出てくる敵一覧。
    /// </summary>
    public ReadOnlySpan<Enemy> AppearedEnemies => _appearedEnemies;
}
