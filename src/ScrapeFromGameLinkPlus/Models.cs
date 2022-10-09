namespace ScrapeFromGameLinkPlus;

record Enemy(string Name, List<Stage> AppearingStages)
{
    public Enemy(string name) : this(name, new()) { }
}
record Stage(string Name, int Index, int Energy, Enemy[] Enemies)
{
    public Section Section { get; internal set; } = default!;
}
record Section(string Name, int Index, Stage[] Stages);
record Story(Section[] Sections, Enemy[] Enemies);
