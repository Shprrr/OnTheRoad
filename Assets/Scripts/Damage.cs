using System;

public struct Damage
{
    public enum DamageType
    {
        HP,
        MP,
        DrainHP,
        DrainMP
    }

    public DamageType Type { get; set; }
    public int Value { get; set; }
    /// <summary>
    /// Number of hits hitted.  Already calculated in Damage.
    /// </summary>
    public int Multiplier { get; set; }
    public Battler User { get; set; }
    public Battler Target { get; set; }

    public string Name { get; set; }

    public Damage(DamageType type, int value, int multiplier, Battler user, Battler target, string name = "")
    {
        Type = type;
        Value = value;
        Multiplier = multiplier;
        User = user;
        Target = target;
        Name = name;
    }

    public void ApplyDamage()//TODO: Change for Attacker
    {
        switch (Type)
        {
            case DamageType.HP:
            case DamageType.DrainHP:
                //if (target != null && target.Statuses.ContainsKey(Status.eStatus.Stone))
                //{
                //    target.Statuses[Status.eStatus.Stone].OnAppliedDamage(target, Value);
                //}
                //else
                {
                    Target.Hp -= Value;
                    if (Type == DamageType.DrainHP)
                        User.Hp += Value;
                }
                break;

            case DamageType.MP:
            case DamageType.DrainMP:
                Target.Sp -= Value;
                if (Type == DamageType.DrainMP)
                    User.Sp += Value;
                break;
        }
    }

    public override string ToString()
    {
        //return Multiplier + "x" + Value + " " + Type;
        return Multiplier == 0 ? "MISS" : Value + " " + Type;
    }

    public override bool Equals(object obj)
    {
        if (obj is Damage)
        {
            var d2 = (Damage)obj;
            return Type == d2.Type && Value == d2.Value && Multiplier == d2.Multiplier && User == d2.User;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(Damage d1, Damage d2)
    {
        return d1.Equals(d2);
    }

    public static bool operator !=(Damage d1, Damage d2)
    {
        return !d1.Equals(d2);
    }

    /// <summary>
    /// Merges two Damage. Need to be the same Type and by the same User.
    /// </summary>
    /// <param name="d1"></param>
    /// <param name="d2"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Damage operator +(Damage d1, Damage d2)
    {
        if (d1 == Empty)
            return d2;
        if (d2 == Empty)
            return d1;
        if (d1.Type != d2.Type || d1.User != d2.User)
            throw new InvalidOperationException("Damages are not compatible for the operation.");

        return new Damage(d1.Type, d1.Value + d2.Value, d1.Multiplier + d2.Multiplier, d1.User, d1.Target, d1.Name);
    }

    public static readonly Damage Empty = new Damage(DamageType.HP, 0, 0, null, null);
}
