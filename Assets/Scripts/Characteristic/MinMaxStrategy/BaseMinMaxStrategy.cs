public class BaseMinMaxStrategy : IMinMaxStrategy
{
    public float GetMinValue(CalculatedTraits calculatedTraits)
    {
        return 0;
    }

    public float GetMaxValue(CalculatedTraits calculatedTraits)
    {
        return 999;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new Characteristic[0];
    }
}
