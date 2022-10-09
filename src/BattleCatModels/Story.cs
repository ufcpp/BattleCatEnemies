namespace BattleCatModels;

/// <summary>
/// 「レジェンド」とか「真レジェンド」とかのレベル。
/// </summary>
public record Story(string Name, Section[] Sections);

/// <summary>
/// 「伝説のはじまり」とか「脆弱性と弱酸性」とかのレベル。
/// </summary>
public record Section(string Name, Stage[] Stages);

/// <summary>
/// 個別のステージ。
/// </summary>
public record Stage(string Name, int Energy);
