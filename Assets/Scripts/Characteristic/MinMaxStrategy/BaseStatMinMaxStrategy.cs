public class BaseStatMinMaxStrategy : IMinMaxStrategy
{
    public float GetMinValue(CalculatedTraits calculatedTraits)
    {
        return 0;
    }

    public float GetMaxValue(CalculatedTraits calculatedTraits)
    {
        return 980;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new Characteristic[0];
    }
}
