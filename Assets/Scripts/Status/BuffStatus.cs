using System;
using UnityEngine;

[Serializable]
public class BuffStatus : Status
{
    public enum Stats
    {
        Strength,
        Vitality,
        Intellect,
        Wisdom,
        Agility
    }

    public Stats Stat;
    public int Level;
    public const int MaxLevel = 3;

    public BuffStatus(Stats stat, int level)
    {
        Stat = stat;
        Level = level;
        Id = "buff" + Stat;

        Update();
    }

    public BuffStatus(BuffStatus status, int turnLeft) : base(status, turnLeft)
    {
        Stat = status.Stat;
        Level = status.Level;
    }

    private void Update()
    {
        switch (Stat)
        {
            case Stats.Strength:
                Name = "Buff Str. L" + Level;
                Description = "Buff strength.";
                break;
            case Stats.Vitality:
                Name = "Buff Vit. L" + Level;
                Description = "Buff vitality.";
                break;
            case Stats.Intellect:
                Name = "Buff Int. L" + Level;
                Description = "Buff intellect.";
                break;
            case Stats.Wisdom:
                Name = "Buff Wis. L" + Level;
                Description = "Buff wisdom.";
                break;
            case Stats.Agility:
                Name = "Buff Agi. L" + Level;
                Description = "Buff agility.";
                break;
        }
    }

    public override bool Stackable(Status other)
    {
        if (Id == other.Id && Level < MaxLevel)
        {
            Level = Mathf.Clamp(Level + ((BuffStatus)other).Level, 0, MaxLevel);
            TurnLeft = Math.Max(TurnLeft, other.TurnLeft);
            Update();
            return true;
        }

        return base.Stackable(other);
    }

    public override Status Copy(int turns)
    {
        return new BuffStatus(this, turns);
    }
}
