using System;

[Serializable]
public class HealEffect : Effect
{
    public int BaseHeal;
    public int BaseAccuracy;
    public Damage.eDamageType DamageType;

    public HealEffect()
    {
    }

    public HealEffect(int baseHeal, int baseAccuracy, Damage.eDamageType damageType = Damage.eDamageType.HP)
    {
        BaseHeal = baseHeal;
        BaseAccuracy = baseAccuracy;
        DamageType = damageType;
    }

    public override bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = user.CalculateMagicalDamage(target, BaseHeal, BaseAccuracy, nbTarget);
        damage.Value *= -1;
        damage.Type = DamageType;
        return true;
    }

    public override string ToString()
    {
        return string.Format("HealEffect({0}, {1}, {2})", BaseHeal, BaseAccuracy, DamageType);
    }
}
