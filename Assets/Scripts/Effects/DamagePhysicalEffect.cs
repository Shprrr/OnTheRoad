using System;

[Serializable]
public class DamagePhysicalEffect : Effect
{
    public int BaseDamage;
    public bool AlwaysHit;
    public Damage.DamageType DamageType;

    public DamagePhysicalEffect()
    {
    }

    public DamagePhysicalEffect(int baseDamage, bool alwaysHit, Damage.DamageType damageType = Damage.DamageType.HP)
    {
        BaseDamage = baseDamage;
        AlwaysHit = alwaysHit;
        DamageType = damageType;
    }

    public override bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = user.CalculatePhysicalDamage(target, BaseDamage, AlwaysHit, nbTarget);
        damage.Type = DamageType;
        return true;
    }

    public override string ToString()
    {
        return string.Format("DamagePhysicalEffect({0}, {1}, {2})", BaseDamage, AlwaysHit, DamageType);
    }
}
