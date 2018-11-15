using System;

[Serializable]
public class StoneStatus : Status
{
    public StoneStatus()
    {
        Id = "stone";
        Name = "Stone";
        Description = "Turned into a rock.";
    }

    public StoneStatus(StoneStatus status, int turnLeft) : base(status, turnLeft)
    {
    }

    public override Status Copy(int turns)
    {
        return new StoneStatus(this, turns);
    }
}
