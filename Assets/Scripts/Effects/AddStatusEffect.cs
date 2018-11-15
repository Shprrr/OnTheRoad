using System;

[Serializable]
public class AddStatusEffect : Effect
{
    public Status Status;
    public int ChanceToApply;
    public int MinTurn;
    public int MaxTurn;

    public AddStatusEffect(Status status, int chanceToApply)
    {
        Status = status;
        ChanceToApply = chanceToApply;
        MinTurn = -1;
        MaxTurn = -1;
    }

    public AddStatusEffect(Status status, int chanceToApply, int minTurn, int maxTurn) : this(status, chanceToApply)
    {
        MinTurn = minTurn;
        MaxTurn = maxTurn;
    }

    public override bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = new Damage(Damage.eDamageType.HP, 0, 0, user, target);
        var status = Status.Copy(UnityEngine.Random.Range(MinTurn, MaxTurn + 1));
        if (UnityEngine.Random.Range(0, 100) < ChanceToApply && target.BattlerStatus.AddStatus(status))
            damage.Multiplier = 1;
        return true;
    }

    public override string ToString()
    {
        return string.Format("AddStatusEffect({0}, {1}, {2}, {3})", Status, ChanceToApply, MinTurn, MaxTurn);
    }
}
