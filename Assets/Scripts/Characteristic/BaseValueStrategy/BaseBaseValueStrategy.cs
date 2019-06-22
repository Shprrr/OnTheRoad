public class BaseBaseValueStrategy : IBaseValueStrategy
{
    public float GetBaseValue(CalculatedTraits calculatedTraits)
    {
        return 0;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new Characteristic[0];
    }
}
