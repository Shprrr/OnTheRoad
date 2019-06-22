public class PhysicalDamageBaseValueStrategy : IBaseValueStrategy
{
    public float GetBaseValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.StrengthId] / 4;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.StrengthId) };
    }
}
