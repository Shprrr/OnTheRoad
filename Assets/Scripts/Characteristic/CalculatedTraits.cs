using System.Collections.Generic;
using UnityEngine;

public class CalculatedTraits
{
    private readonly Dictionary<Characteristic, float> _calculatedTraits;

    public CalculatedTraits()
    {
        _calculatedTraits = new Dictionary<Characteristic, float>();
    }

    public float this[Characteristic characteristic]
    {
        get
        {
            if (_calculatedTraits.ContainsKey(characteristic))
                return _calculatedTraits[characteristic];

            return Mathf.Clamp(characteristic.BaseValueStrategy.GetBaseValue(this), characteristic.MinMaxStrategy.GetMinValue(this), characteristic.MinMaxStrategy.GetMaxValue(this));
        }
        set
        {
            if (_calculatedTraits.ContainsKey(characteristic))
                _calculatedTraits[characteristic] = value;
            else
                _calculatedTraits.Add(characteristic, value);
        }
    }

    public float this[string characteristicId]
    {
        get
        {
            return this[CharacteristicFactory.Build(characteristicId)];
        }
        set
        {
            this[CharacteristicFactory.Build(characteristicId)] = value;
        }
    }
}
