using System;

[Serializable]
public class DamagePhysicalEffect : Effect
{
    public int BaseDamage;
    public bool AlwaysHit;
    public Damage.eDamageType DamageType;

    public DamagePhysicalEffect()
    {
    }

    public DamagePhysicalEffect(int baseDamage, bool alwaysHit, Damage.eDamageType damageType = Damage.eDamageType.HP)
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
}
