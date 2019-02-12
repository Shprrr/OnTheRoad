using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Parameters;
using static SecondaryParameters;
using static TraitKey;

[Serializable]
public class Trait
{
    [SerializeField]
    [HideInInspector]
    private string Name; private void ChangeName() => Name = Key.ToString();
    public TraitKey Key;
    public MinMaxValue MinMaxValue;

    public Trait(TraitKey key, int min, int max, int value) : this(key, new MinMaxValue(min, max, value))
    {
    }

    public Trait(TraitKey key, MinMaxValue value)
    {
        Key = key; ChangeName();
        MinMaxValue = value;
    }

    public override string ToString()
    {
        return Key.ToString();
    }

    public static Trait BuildParamTrait(ParameterIndex id, int value)
    {
        return new Trait(TraitKey.BuildParamTrait(id), 0, 1000, value);
    }

    public static Trait BuildSecondaryParamTrait(SecondaryParameterIndex id, int value)
    {
        int min = -100, max = 100;
        return new Trait(TraitKey.BuildSecondaryParamTrait(id), min, max, value);
    }

    public static Trait BuildSpecialParamTrait(Traits.SpecialParameterIndex id, int value)
    {
        return new Trait(TraitKey.BuildSpecialParamTrait(id), 0, 1000, value);
    }
}

[Serializable]
public struct TraitKey
{
    public enum TraitIndex
    {
        ParamRate,
        SecondaryParamRate,
        SpecialParamRate
    }

    [SerializeField]
    private string Name;
    [SerializeField]
    private TraitIndex traitId;
    public int DataId;

    public TraitIndex TraitId { get => traitId; private set => traitId = value; }

    public TraitKey(TraitIndex traitId)
    {
        this.traitId = traitId;
        Name = traitId.ToString();
        DataId = -1;
    }

    public TraitKey(TraitIndex traitId, int dataId) : this(traitId)
    {
        DataId = dataId;
    }

    public override string ToString()
    {
        return Name + (DataId > -1 ? " " + DataId : "");
    }

    public override bool Equals(object obj)
    {
        if (!(obj is TraitKey))
        {
            return false;
        }

        var key = (TraitKey)obj;
        return traitId == key.traitId &&
               DataId == key.DataId;
    }

    public override int GetHashCode()
    {
        var hashCode = -542267402;
        hashCode = hashCode * -1521134295 + traitId.GetHashCode();
        hashCode = hashCode * -1521134295 + DataId.GetHashCode();
        return hashCode;
    }

    public static bool operator ==(TraitKey key1, TraitKey key2)
    {
        return key1.Equals(key2);
    }

    public static bool operator !=(TraitKey key1, TraitKey key2)
    {
        return !(key1 == key2);
    }

    public static TraitKey BuildParamTrait(ParameterIndex id)
    {
        return new TraitKey(TraitIndex.ParamRate, (int)id);
    }

    public static TraitKey BuildSecondaryParamTrait(SecondaryParameterIndex id)
    {
        return new TraitKey(TraitIndex.SecondaryParamRate, (int)id);
    }

    public static TraitKey BuildSpecialParamTrait(Traits.SpecialParameterIndex id)
    {
        return new TraitKey(TraitIndex.SpecialParamRate, (int)id);
    }
}

[Serializable]
public class Traits : ISerializationCallbackReceiver
{
    public enum SpecialParameterIndex
    {
        ExperienceRate
    }

    [SerializeField]
    private List<Trait> _traits = new List<Trait>();
    // Unity doesn't know how to serialize a Dictionary
    private Dictionary<TraitKey, MinMaxValue> traits = new Dictionary<TraitKey, MinMaxValue>();

    public MinMaxValue this[TraitIndex index]
    {
        get { return traits[new TraitKey(index)]; }
    }

    public MinMaxValue this[TraitIndex index, int id]
    {
        get { return traits[new TraitKey(index, id)]; }
    }

    public MinMaxValue this[ParameterIndex id]
    {
        get { return traits[BuildParamTrait(id)]; }
    }

    public MinMaxValue this[SecondaryParameterIndex id]
    {
        get { return traits[BuildSecondaryParamTrait(id)]; }
    }

    public MinMaxValue this[SpecialParameterIndex id]
    {
        get { return traits[BuildSpecialParamTrait(id)]; }
    }

    public Traits()
    {
        var indexes = Enum.GetValues(typeof(TraitIndex));
        for (int i = 0; i < indexes.Length; i++)
        {
            var index = (TraitIndex)indexes.GetValue(i);
            switch (index)
            {
                case TraitIndex.ParamRate:
                    var parameters = Enum.GetValues(typeof(ParameterIndex)).Cast<ParameterIndex>().ToArray();
                    //var parameters = new Parameters();
                    for (int p = 0; p < parameters.Length; p++)
                    {
                        var trait = Trait.BuildParamTrait(parameters[p], 1);
                        traits.Add(trait.Key, trait.MinMaxValue);
                    }
                    break;
                case TraitIndex.SecondaryParamRate:
                    var secondaryParameters = Enum.GetValues(typeof(SecondaryParameterIndex)).Cast<SecondaryParameterIndex>().ToArray();
                    for (int p = 0; p < secondaryParameters.Length; p++)
                    {
                        int defaultValue = 0;
                        switch (secondaryParameters[p])
                        {
                            case SecondaryParameterIndex.PhysicalHitRate:
                                defaultValue = 80;
                                break;
                        }
                        var trait = Trait.BuildSecondaryParamTrait(secondaryParameters[p], defaultValue);
                        traits.Add(trait.Key, trait.MinMaxValue);
                    }
                    break;
                case TraitIndex.SpecialParamRate:
                    var specialParameters = Enum.GetValues(typeof(SpecialParameterIndex)).Cast<SpecialParameterIndex>().ToArray();
                    for (int p = 0; p < specialParameters.Length; p++)
                    {
                        var trait = Trait.BuildSpecialParamTrait(specialParameters[p], 100);
                        traits.Add(trait.Key, trait.MinMaxValue);
                    }
                    break;
                default:
                    traits.Add(new TraitKey(index), new MinMaxValue());
                    break;
            }
        }
    }

    public void OnBeforeSerialize()
    {
        _traits.Clear();

        foreach (var kvp in traits)
        {
            _traits.Add(new Trait(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        traits = new Dictionary<TraitKey, MinMaxValue>();

        for (int i = 0; i < _traits.Count; i++)
        {
            traits.Add(_traits[i].Key, _traits[i].MinMaxValue);
        }
    }
}
