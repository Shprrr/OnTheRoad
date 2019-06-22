using System;
using System.Linq;

[Serializable]
public class StatusType : IData
{
    public string _id;
    public string _name;
    public string _description;
    public int MaxLevel;
    public bool IsAlive;
    public RestrictionType? Restriction;
    public ILookup<int, Trait> TraitsByLevel;

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }

    public StatusType(string id, string name, int maxLevel, string description, TraitByLevel[] traits, RestrictionType? restriction = null, bool isAlive = true)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        MaxLevel = maxLevel;
        IsAlive = isAlive;
        Restriction = restriction;
        TraitsByLevel = traits.ToLookup(t => t.Level, t => t.Trait);
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public Status CreateStatus(int nbTurns, int level = 1)
    {
        return new Status(this, level, nbTurns);
    }

    public override string ToString()
    {
        return Name;
    }
}

public struct TraitByLevel
{
    public int Level;
    public Trait Trait;

    public TraitByLevel(int level, string characteristicId, int value, TraitOperator @operator)
    {
        Level = level;
        Trait = new Trait(characteristicId, value, @operator);
    }

    public override string ToString()
    {
        return "L" + Level + ":" + Trait;
    }
}

public enum RestrictionType
{
    CannotMove,
    AttackEveryone,
    AttackAlly,
    AttackEnemy
}
