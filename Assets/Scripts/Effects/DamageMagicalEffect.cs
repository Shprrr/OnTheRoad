using System;

[Serializable]
public class DamageMagicalEffect : Effect
{
    public int BaseDamage;
    public int BaseAccuracy;
    public Damage.eDamageType DamageType;

    public DamageMagicalEffect()
    {
    }

    public DamageMagicalEffect(int baseDamage, int baseAccuracy, Damage.eDamageType damageType = Damage.eDamageType.HP)
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
}
