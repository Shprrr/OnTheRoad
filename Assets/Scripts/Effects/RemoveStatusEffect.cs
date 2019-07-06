using System;

[Serializable]
public class RemoveStatusEffect : Effect
{
    public StatusType[] Type;
    public int ChanceToApply;

    public RemoveStatusEffect(int chanceToApply, params StatusType[] status)
    {
        Type = status ?? throw new ArgumentNullException(nameof(status));
        ChanceToApply = chanceToApply;
    }

    public override bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = new Damage(Damage.DamageType.HP, 0, 0, user, target);
        if (UnityEngine.Random.Range(0, 100) < ChanceToApply && target.BattlerStatus.RemoveStatus(Type))
            damage.Multiplier = 1;
        return true;
    }

    public override string ToString()
    {
        return string.Format("RemoveStatusEffect({0}, {1})", Type, ChanceToApply);
    }
}
