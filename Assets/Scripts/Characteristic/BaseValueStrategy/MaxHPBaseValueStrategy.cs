public class MaxHPBaseValueStrategy : IBaseValueStrategy
{
    public float GetBaseValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.VitalityId] / 4 + 10;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.VitalityId) };
    }
}
