public class BooleanMinMaxStrategy : IMinMaxStrategy
{
    public float GetMinValue(CalculatedTraits calculatedTraits)
    {
        return 0;
    }

    public float GetMaxValue(CalculatedTraits calculatedTraits)
    {
        return 1;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new Characteristic[0];
    }
}
