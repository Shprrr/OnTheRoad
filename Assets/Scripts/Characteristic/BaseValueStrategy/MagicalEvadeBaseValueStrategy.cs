public class MagicalEvadeBaseValueStrategy : IBaseValueStrategy
{
    public float GetBaseValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.AgilityId] / 8 + calculatedTraits[CharacteristicFactory.WisdomId] / 8;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.AgilityId), CharacteristicFactory.Build(CharacteristicFactory.WisdomId) };
    }
}
