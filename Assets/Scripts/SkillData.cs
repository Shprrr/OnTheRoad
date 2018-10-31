using System;
using System.Collections.Generic;
using static Cursor;

[Serializable]
public class SkillData : IDataEffect, IEquatable<SkillData>
{
    public string _id;
    public string _name;
    public string _description;
    public string _animationName;
    public eTargetType[] _targetsPossible;
    public Effect _effect;
    public int SpCost;
    public int CTBRank;

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public string AnimationName { get { return _animationName; } set { _animationName = value; } }
    public eTargetType[] TargetsPossible { get { return _targetsPossible; } set { _targetsPossible = value; } }
    public Effect Effect { get { return _effect; } set { _effect = value; } }

    public SkillData()
    {
    }

    public SkillData(string id, string name, int spCost, string animationName, eTargetType[] targetsPossible, int ctbRank, Effect effect, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        SpCost = spCost;
        CTBRank = ctbRank;
        AnimationName = animationName;
        TargetsPossible = targetsPossible;
        Effect = effect;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as SkillData);
    }

    public bool Equals(SkillData other)
    {
        return other != null &&
               Id == other.Id;
    }

    public override int GetHashCode()
    {
        return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
    }

    public override string ToString()
    {
        return Name;
    }

    public static bool operator ==(SkillData skill1, SkillData skill2) => skill1?.Id == skill2?.Id;

    public static bool operator !=(SkillData skill1, SkillData skill2) => !(skill1 == skill2);
}
