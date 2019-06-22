using System;

[Serializable]
public class AddStatusEffect : Effect
{
    public StatusType Type;
    public int ChanceToApply;
    public int Level;
    public int MinTurn;
    public int MaxTurn;

    public AddStatusEffect(StatusType status, int chanceToApply)
    {
        Type = status;
        ChanceToApply = chanceToApply;
        Level = 1;
        MinTurn = -1;
        MaxTurn = -1;
    }

    public AddStatusEffect(StatusType status, int chanceToApply, int minTurn, int maxTurn) : this(status, chanceToApply)
    {
        MinTurn = minTurn;
        MaxTurn = maxTurn;
    }

    public AddStatusEffect(StatusType status, int chanceToApply, int level, int minTurn, int maxTurn) : this(status, chanceToApply, minTurn, maxTurn)
    {
        Level = level;
    }

    public override bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = new Damage(Damage.DamageType.HP, 0, 0, user, target);
        var status = Type.CreateStatus(UnityEngine.Random.Range(MinTurn, MaxTurn + 1), Level);
        if (UnityEngine.Random.Range(0, 100) < ChanceToApply && target.BattlerStatus.AddStatus(status))
            damage.Multiplier = 1;
        return true;
    }

    public override string ToString()
    {
        return string.Format("AddStatusEffect({0}, {1}, {2}, {3})", Type, ChanceToApply, MinTurn, MaxTurn);
    }
}
