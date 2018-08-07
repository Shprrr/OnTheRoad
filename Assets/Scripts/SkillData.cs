using System;
using static Cursor;

[Serializable]
public class SkillData
{
    public string Name;
    public int SpCost;
    public string AnimationName;
    public eTargetType[] TargetsPossible;
    public Effect Effect;

    public SkillData()
    {
    }

    public SkillData(string name, int spCost, string animationName, eTargetType[] targetsPossible, Effect effect)
    {
        Name = name;
        SpCost = spCost;
        AnimationName = animationName;
        TargetsPossible = targetsPossible;
        Effect = effect;
    }
}
