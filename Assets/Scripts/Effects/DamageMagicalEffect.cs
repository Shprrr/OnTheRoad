using System;

[Serializable]
public class DamageMagicalEffect : Effect
{
    public int BaseDamage;
    public int BaseAccuracy;
    public Damage.DamageType DamageType;

    public DamageMagicalEffect()
    {
    }

    public DamageMagicalEffect(int baseDamage, int baseAccuracy, Damage.DamageType damageType = Damage.DamageType.HP)
    {
        BaseDamage = baseDamage;
        BaseAccuracy = baseAccuracy;
        DamageType = damageType;
    }

    public override bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = user.CalculateMagicalDamage(target, BaseDamage, BaseAccuracy, nbTarget);
        damage.Type = DamageType;
        return true;
    }

    public override string ToString()
    {
        return string.Format("DamageMagicalEffect({0}, {1}, {2})", BaseDamage, BaseAccuracy, DamageType);
    }
}
