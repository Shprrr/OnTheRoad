using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Status
{
    public StatusType Type { get; private set; }
    public int Level;
    public int TurnLeft;

    public Status(StatusType type, int level, int turnLeft)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Level = level;
        TurnLeft = turnLeft;
    }

    public bool StackStatus(Status other)
    {
        if (Type != other.Type) return false;

        if (Level < Type.MaxLevel)
        {
            Level = Mathf.Clamp(Level + other.Level, 0, Type.MaxLevel);
            TurnLeft = Math.Max(TurnLeft, other.TurnLeft);
            return true;
        }

        if (TurnLeft < other.TurnLeft)
        {
            TurnLeft = other.TurnLeft;
            return true;
        }

        return false;
    }

    public IEnumerable<Trait> GetTraits()
    {
        return Type.TraitsByLevel[Level];
    }

    public override string ToString()
    {
        if (Type.MaxLevel > 1)
            return Type.Name + " L" + Level;

        return Type.Name;
    }
}