using System;
using System.Collections.Generic;
using static Cursor;

[Serializable]
public class SkillData : Data, IEquatable<SkillData>
{
    public int SpCost;

    public SkillData()
    {
    }

    public SkillData(string id, string name, int spCost, string animationName, eTargetType[] targetsPossible, Effect effect, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        SpCost = spCost;
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
