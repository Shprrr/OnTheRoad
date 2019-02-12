using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Parameter
{
    [SerializeField]
    private string Name;
    public int MinValue;
    public int MaxValue;
    public int BaseValue;
    public int PlusValue;
    public float BuffRate = 1;

    [ReadOnlyProperty(nameof(Value))]
    [SerializeField]
    private int value;
    public int Value { get { return (int)Math.Round(Mathf.Clamp((BaseValue + PlusValue) * BuffRate, MinValue, MaxValue)); } }

    public Parameter(string name, int minValue, int maxValue, int baseValue, int plusValue, float buffRate = 1)
    {
        Name = name;
        MinValue = minValue;
        MaxValue = maxValue;
        BaseValue = baseValue;
        PlusValue = plusValue;
        BuffRate = buffRate;
    }

    public override string ToString()
    {
        return Name;
    }
}

[Serializable]
public class Parameters : IReadOnlyList<Parameter>
{
    public enum ParameterIndex
    {
        MaxHp,
        MaxSp,
        Strength,
        Vitality,
        Intellect,
        Wisdom,
        Agility
    }

    public const int MinMaxHp = 1;
    public const int MaxMaxHp = 999999;
    public const int MaxMaxSp = 999;
    public const int MinStats = 0;// 1 ?
    public const int MaxStats = 999;

    [SerializeField]
    private Parameter[] parameters;

    public int Count => parameters.Length;

    public Parameter this[int index] => parameters[index];

    public Parameter this[ParameterIndex index]
    {
        get { return parameters[(int)index]; }
    }

    public Parameters()
    {
        var names = Enum.GetNames(typeof(ParameterIndex));
        parameters = new Parameter[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            parameters[i] = new Parameter(names[i], 0, 0, 0, 0);
        }

        this[ParameterIndex.MaxHp].MinValue = MinMaxHp;
        this[ParameterIndex.MaxHp].MaxValue = MaxMaxHp;
        this[ParameterIndex.MaxHp].BaseValue = 1;

        this[ParameterIndex.MaxSp].MaxValue = MaxMaxSp;

        this[ParameterIndex.Strength].MaxValue = MaxStats;
        this[ParameterIndex.Strength].MinValue = MinStats;
        this[ParameterIndex.Vitality].MaxValue = MaxStats;
        this[ParameterIndex.Vitality].MinValue = MinStats;
        this[ParameterIndex.Intellect].MaxValue = MaxStats;
        this[ParameterIndex.Intellect].MinValue = MinStats;
        this[ParameterIndex.Wisdom].MaxValue = MaxStats;
        this[ParameterIndex.Wisdom].MinValue = MinStats;
        this[ParameterIndex.Agility].MaxValue = MaxStats;
        this[ParameterIndex.Agility].MinValue = MinStats;
    }

    public IEnumerator<Parameter> GetEnumerator()
    {
        return ((IReadOnlyList<Parameter>)parameters).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return parameters.GetEnumerator();
    }

    public void ChangeValue(ParameterIndex index, int value)
    {
        parameters[(int)index].BaseValue = value;
    }
}


[Serializable]
public class SecondaryParameters : IReadOnlyList<MinMaxValue>
{
    public enum SecondaryParameterIndex
    {
        PhysicalMinDamage,
        PhysicalMaxDamage,
        PhysicalAttackMultiplier,
        PhysicalHitRate,
        PhysicalDefense,
        PhysicalDefenseMultiplier,
        PhysicalEvadeRate
    }

    public const int MinDamage = 1;
    public const int MaxDamage = 999;
    public const int MinMultiplier = 1;
    public const int MaxMultiplier = 16;
    public const int MinHitRate = 0;
    public const int MaxHitRate = 100;
    public const int DefaultPhysicalHitRate = 80;
    public const int MinDefense = 0;
    public const int MaxDefense = 999;

    [SerializeField]
    private MinMaxValue[] parameters;

    public int Count => parameters.Length;

    public MinMaxValue this[int index] => parameters[index];

    public MinMaxValue this[SecondaryParameterIndex index]
    {
        get { return parameters[(int)index]; }
        protected set { parameters[(int)index] = value; }
    }

    public SecondaryParameters()
    {
        var names = Enum.GetNames(typeof(SecondaryParameterIndex));
        parameters = new MinMaxValue[names.Length];

        this[SecondaryParameterIndex.PhysicalMinDamage] = new MinMaxValue(MinDamage, MaxDamage, MinDamage);
        this[SecondaryParameterIndex.PhysicalMaxDamage] = new MinMaxValue(MinDamage, MaxDamage, MinDamage);
        this[SecondaryParameterIndex.PhysicalAttackMultiplier] = new MinMaxValue(MinMultiplier, MaxMultiplier, MinMultiplier);
        this[SecondaryParameterIndex.PhysicalHitRate] = new MinMaxValue(MinHitRate, MaxHitRate, DefaultPhysicalHitRate);
        this[SecondaryParameterIndex.PhysicalDefense] = new MinMaxValue(MinDefense, MaxDefense, MinDefense);
        this[SecondaryParameterIndex.PhysicalDefenseMultiplier] = new MinMaxValue(MinMultiplier, MaxMultiplier, MinMultiplier);
        this[SecondaryParameterIndex.PhysicalEvadeRate] = new MinMaxValue(MinHitRate, MaxHitRate, MinHitRate);
    }

    public IEnumerator<MinMaxValue> GetEnumerator()
    {
        return ((IReadOnlyList<MinMaxValue>)parameters).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return parameters.GetEnumerator();
    }

    public void ChangeValue(SecondaryParameterIndex index, int value)
    {
        parameters[(int)index].Value = value;
    }

    public int SumAllTrait(IEnumerable<Trait> traits, TraitKey key)
    {
        if (key.TraitId != TraitKey.TraitIndex.SecondaryParamRate) return 0;

        var minMaxValue = this[key.DataId];
        minMaxValue.Value += traits.Where(t => t.Key == key).Sum(t => t.MinMaxValue.Value);

        return minMaxValue.Value;
    }
}
