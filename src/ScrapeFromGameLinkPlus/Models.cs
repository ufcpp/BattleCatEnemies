namespace ScrapeFromGameLinkPlus;

record Enemy(string Name, List<Stage> AppearingStages)
{
    public Enemy(string name) : this(name, new()) { }
}
record Stage(string Name, int Index, int Stamina, Enemy[] Enemies);
record Section(string Name, int Index, Stage[] Stages);
record NyankoData(Section[] Sections, Enemy[] Enemies);
