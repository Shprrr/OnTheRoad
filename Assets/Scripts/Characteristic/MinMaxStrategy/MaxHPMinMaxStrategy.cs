public class MaxHPMinMaxStrategy : IMinMaxStrategy
{
    public float GetMinValue(CalculatedTraits calculatedTraits)
    {
        return 1;
    }

    public float GetMaxValue(CalculatedTraits calculatedTraits)
    {
        return 999999;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new Characteristic[0];
    }
}
