using System;

[Serializable]
public class DamageEffect : Effect
{
    public int BaseDamage;
    public int BaseAccuracy;

    public DamageEffect()
    {
    }

    public DamageEffect(int baseDamage, int baseAccuracy)
    {
        BaseDamage = baseDamage;
        BaseAccuracy = baseAccuracy;
    }

    public override bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = user.CalculateMagicalDamage(target, BaseDamage, BaseAccuracy, nbTarget);
        return true;
    }
}
