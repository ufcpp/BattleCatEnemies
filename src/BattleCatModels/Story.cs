namespace BattleCatModels;

// 「レジェンド」とか「真レジェンド」とかのレベル。
public record Story(string Name, Section[] Sections);

// 「伝説のはじまり」とか「脆弱性と弱酸性」とかのレベル。
public record Section(string Name, Stage[] Stages);

// 個別のステージ。
public record Stage(string Name, int Energy);

// インデックスだけあれば「何セクション目の何ステージ」が特定できるので int で参照。
public record struct StageRef(int Section, int Stage);

// 敵がどのステージに出現するか。
public record EnemyAppearance(int EnemyId, StageRef[] Stages);
