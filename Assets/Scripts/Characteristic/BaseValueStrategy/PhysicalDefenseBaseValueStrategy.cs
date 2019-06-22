public class PhysicalDefenseBaseValueStrategy : IBaseValueStrategy
{
    public float GetBaseValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.VitalityId] / 2;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.VitalityId) };
    }
}
