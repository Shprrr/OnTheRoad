using System;

[Serializable]
public class GuardStatus : Status
{
    public GuardStatus()
    {
        Id = "guard";
        Name = "Guard";
        Description = "Defensive stance that augments physical and magical defenses while lowering evasions.";
    }

    public GuardStatus(GuardStatus status, int turnLeft) : base(status, turnLeft)
    {
    }

    public override Status Copy(int turns)
    {
        return new GuardStatus(this, turns);
    }
}
