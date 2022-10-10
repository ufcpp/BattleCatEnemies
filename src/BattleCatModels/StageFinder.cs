using BattleCat.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace BattleCat;

public class StageFinder
{
    public Story Story { get; }
    private readonly Enemy[] _appearedEnemies;
    private readonly Dictionary<int, EnemyAppearance> _enemyMap;

    public record struct Entry(int SectionId, Section Section, int StageId, Stage Stage)
    {
        public int Energy() => Stage.Energy;
        public override string ToString() => $"{Section.Name} - {Stage.Name} ({Stage.Energy})";
    }

    public record EntryArray(Array3<Entry> Entries)
    {
        public int Energy()
        {
            var sum = 0;
            for (int i = 0; i < Entries.Length; i++) sum += Entries[i].Energy();
            return sum;
        }

        public override string ToString() => Entries.Length switch
        {
            0 => "",
            1 => Entries[0].ToString(),
            2 => $"{Entries[0]} + {Entries[1]}",
            _ => $"{Entries[0]} + {Entries[1]} + {Entries[2]}",
        };
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

    private StageRef[]? FindSingle(int enemyId)
        => _enemyMap.TryGetValue(enemyId, out var e) ? e.Stages : null;

    /// <summary>
    /// 敵が出るステージを検索。
    /// </summary>
    public IEnumerable<EntryArray> Find(int take, params int[] enemyIds)
        => FindRaw(enemyIds).Select(Resolve)
        .OrderBy(x => x.Energy())
        .Take(take);

    /// <summary>
    /// 敵が出るステージを検索。
    /// </summary>
    public IEnumerable<Array3<StageRef>> FindRaw(params int[] enemyIds) => enemyIds.Length switch
    {
        1 => FindRaw(enemyIds[0]),
        2 => FindRaw(enemyIds[0], enemyIds[1]),
        3 => FindRaw(enemyIds[0], enemyIds[1], enemyIds[2]),
        _ => Array.Empty<Array3<StageRef>>(),
    };

    /// <summary>
    /// 敵が出るステージを検索。
    /// </summary>
    public IEnumerable<Array3<StageRef>> FindRaw(int enemyId1)
    {
        var stages1 = FindSingle(enemyId1);

        if (stages1 is null) yield break;

        foreach (var x in stages1)
        {
            yield return new(x);
        }
    }

    /// <summary>
    /// 2種の敵が同時に出るステージを検索。
    /// </summary>
    public IEnumerable<Array3<StageRef>> FindRaw(int enemyId1, int enemyId2)
    {
        var stages1 = FindSingle(enemyId1);
        var stages2 = FindSingle(enemyId2);

        if (stages1 is null || stages2 is null) yield break;

        foreach (var x in stages1.Intersect(stages2))
            yield return new(x);

        foreach (var x in stages1)
            foreach (var y in stages2.Except(stages1))
                yield return new Array3<StageRef>(x, y).Sort();
    }

    /// <summary>
    /// 3種の敵が同時に出るステージを検索。
    /// </summary>
    /// <remarks>
    /// 最大3種まで同時にミッションに現れるので、3引数まで用意。
    /// </remarks>
    public IEnumerable<Array3<StageRef>> FindRaw(int enemyId1, int enemyId2, int enemyId3)
    {
        var stages1 = FindSingle(enemyId1);
        var stages2 = FindSingle(enemyId2);
        var stages3 = FindSingle(enemyId3);

        if (stages1 is null || stages2 is null || stages3 is null) yield break;

        var s12 = stages1.Intersect(stages2);
        var s23 = stages2.Intersect(stages3);
        var s31 = stages3.Intersect(stages1);

        foreach (var x in s12.Intersect(stages3))
            yield return new(x);

        IEnumerable<Array3<StageRef>> inner()
        {
            foreach (var x in stages1)
                foreach (var y in s23.Except(stages1))
                    yield return new Array3<StageRef>(x, y).Sort();

            foreach (var x in stages2)
                foreach (var y in s31.Except(stages2))
                    yield return new Array3<StageRef>(x, y).Sort();

            foreach (var x in stages3)
                foreach (var y in s12.Except(stages3))
                    yield return new Array3<StageRef>(x, y).Sort();
        }
        foreach (var x in inner().Distinct(StageRefArrayComparer.Instance))
            yield return x;

        foreach (var x in stages1)
            foreach (var y in stages2.Except(stages1))
                foreach (var z in stages3.Except(stages1).Except(stages2))
                    yield return new Array3<StageRef>(x, y, z).Sort();
    }

    private class StageRefArrayComparer : IEqualityComparer<Array3<StageRef>>
    {
        public static readonly StageRefArrayComparer Instance = new();

        public bool Equals(Array3<StageRef> x, Array3<StageRef> y)
        {
            if(x.Length != y.Length) return false;
            for (int i = 0; i < x.Length; i++)
                if (x[i] != y[i]) return false;
            return true;
        }

        public int GetHashCode([DisallowNull] Array3<StageRef> obj)
        {
            var h = new HashCode();
            for (int i = 0; i<obj.Length; i++) h.Add(obj[i].GetHashCode());
            return h.ToHashCode();
        }
    }

    public Entry Resolve(StageRef stageRef)
    {
        var section = Story.Sections[stageRef.Section];
        var stage = section.Stages[stageRef.Stage];
        return new(stageRef.Section + 1, section, stageRef.Stage + 1, stage);
    }

    public EntryArray Resolve(Array3<StageRef> stageRef)
        => new(stageRef.Sort().Select(Resolve));

    /// <summary>
    /// ストーリー中に出てくる敵一覧。
    /// </summary>
    public ReadOnlySpan<Enemy> AppearedEnemies => _appearedEnemies;
}
