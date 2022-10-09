namespace BattleCatModels;

/// <summary>
/// 敵がどのステージに出現するか。
/// </summary>
public record EnemyAppearance(int EnemyId, StageRef[] Stages);

/// <summary>
/// インデックスだけあれば「何 <see name="Section"/> 目の何 <see cref="Stage"/>」が特定できるので int で参照。
/// </summary>
public record struct StageRef(int Section, int Stage);
