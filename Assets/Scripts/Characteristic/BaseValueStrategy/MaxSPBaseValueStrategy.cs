public class MaxSPBaseValueStrategy : IBaseValueStrategy
{
    public float GetBaseValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.WisdomId] / 4 + 5;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.WisdomId) };
    }
}
