using System;

[Serializable]
public class Parameter
{
    public int Value;
    public int MinValue;
    public int MaxValue;

    public Parameter(int value, int minValue, int maxValue)
    {
        Value = value;
        MinValue = minValue;
        MaxValue = maxValue;
    }
}
