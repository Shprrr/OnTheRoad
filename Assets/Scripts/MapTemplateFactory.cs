using System.Linq;

public static class MapTemplateFactory
{
    private static readonly MapTemplateData[] datas = new MapTemplateData[]
    {
        new MapTemplateData("test", "Test Map", "", typeof(MapGeneratorTest), 1, 3, 0, 1, new PossibleEnemyData[] { new PossibleEnemyData("Rat", 3), new PossibleEnemyData("Snake", 2) }),
        new MapTemplateData("tutorial", "Tutorial", "To learn the game", typeof(TutorialMapGenerator), 1, 1, 0, 0, new PossibleEnemyData[] { new PossibleEnemyData("Rat", 3), new PossibleEnemyData("Snake", 3) }),
        new MapTemplateData("greenhill", "Green Hill", "Grassy hills", typeof(GreenHillMapGenerator), 1, 10, -4, 4, new PossibleEnemyData[] { new PossibleEnemyData("Rat", 3) })
    };

    public static MapTemplateData Build(string id)
    {
        return new MapTemplateData(datas.Single(s => s.Id == id));
    }
}
