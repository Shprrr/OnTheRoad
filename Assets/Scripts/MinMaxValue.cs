using System;
using UnityEngine;

[Serializable]
public struct MinMaxValue
{
    public int MinValue;
    public int MaxValue;

    [ReadOnlyProperty(nameof(Value))]
    [SerializeField]
    private int value;

    public int Value { get => value; set => this.value = Mathf.Clamp(value, MinValue, MaxValue); }

    public MinMaxValue(int minValue, int maxValue, int value) : this()
    {
        MinValue = minValue;
        MaxValue = maxValue;
        Value = value;
    }
}
