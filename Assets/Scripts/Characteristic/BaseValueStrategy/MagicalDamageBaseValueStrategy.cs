public class MagicalDamageBaseValueStrategy : IBaseValueStrategy
{
    public float GetBaseValue(CalculatedTraits calculatedTraits)
    {
        return calculatedTraits[CharacteristicFactory.IntellectId] / 8 + calculatedTraits[CharacteristicFactory.WisdomId] / 8;
    }

    public Characteristic[] CharacteristicsDependents()
    {
        return new[] { CharacteristicFactory.Build(CharacteristicFactory.IntellectId), CharacteristicFactory.Build(CharacteristicFactory.WisdomId) };
    }
}
