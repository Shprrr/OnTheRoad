using System;

[Serializable]
public class Effect
{
    public virtual bool CalculateDamage(Battler user, Battler target, out Damage damage, int nbTarget = 1)
    {
        damage = Damage.Empty;
        return false;
    }
}
