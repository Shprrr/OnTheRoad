public class RegenMinMaxStrategy : IMinMaxStrategy
{
    public float GetMinValue(CalculatedTraits calculatedTraits)
    {
        return -999;
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
