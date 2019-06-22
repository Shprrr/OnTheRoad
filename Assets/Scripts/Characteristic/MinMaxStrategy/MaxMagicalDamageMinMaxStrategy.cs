public class MaxMagicalDamageMinMaxStrategy : IMinMaxStrategy
{
    public float GetMinValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.MagMinDamageId];
    }

    public float GetMaxValue(CalculatedTraits calculatedTraits)
    {
        return 999;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.MagMinDamageId) };
    }
}
