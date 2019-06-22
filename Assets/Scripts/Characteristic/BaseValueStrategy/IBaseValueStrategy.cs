public interface IBaseValueStrategy
{
    float GetBaseValue(CalculatedTraits calculatedTraits);
    Characteristic[] CharacteristicsDependents();
}
