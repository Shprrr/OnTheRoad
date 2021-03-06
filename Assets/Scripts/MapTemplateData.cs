using System;

[Serializable]
public class MapTemplateData : IData
{
    public string _id;
    public string _name;
    public string _description;
    public Type Generator;
    public int MinDifficulty;
    public int MaxDifficulty;
    public int BaseCost;
    public int CostByDifficulty;
    public PossibleEnemyData[] PossibleEnemiesData;

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }

    public MapTemplateData(string id) : this(MapTemplateFactory.Build(id))
    {
    }

    public MapTemplateData(string id, string name, string description, Type generator, int minDifficulty, int maxDifficulty, int baseCost, int costByDifficulty, PossibleEnemyData[] possibleEnemiesData)
    {
        Id = id;
        Name = name;
        Description = description;
        Generator = generator;
        MinDifficulty = minDifficulty;
        MaxDifficulty = maxDifficulty;
        BaseCost = baseCost;
        CostByDifficulty = costByDifficulty;
        PossibleEnemiesData = possibleEnemiesData;
    }

    public MapTemplateData(MapTemplateData mapTemplate)
    {
        Id = mapTemplate.Id;
        Name = mapTemplate.Name;
        Description = mapTemplate.Description;
        Generator = mapTemplate.Generator;
        MinDifficulty = mapTemplate.MinDifficulty;
        MaxDifficulty = mapTemplate.MaxDifficulty;
        BaseCost = mapTemplate.BaseCost;
        CostByDifficulty = mapTemplate.CostByDifficulty;
        PossibleEnemiesData = mapTemplate.PossibleEnemiesData;
    }

    public override string ToString()
    {
        return Name;
    }
}

public interface IMapGenerator
{
    void Generate(int difficulty);
}
