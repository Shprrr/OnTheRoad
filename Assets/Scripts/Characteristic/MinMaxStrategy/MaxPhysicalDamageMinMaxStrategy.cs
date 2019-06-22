public class MaxPhysicalDamageMinMaxStrategy : IMinMaxStrategy
{
    public float GetMinValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.PhysMinDamageId];
    }

    public float GetMaxValue(CalculatedTraits calculatedTraits)
    {
        return 999;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.PhysMinDamageId) };
    }
}
