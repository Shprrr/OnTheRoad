public interface IMinMaxStrategy
{
    float GetMinValue(CalculatedTraits calculatedTraits);
    float GetMaxValue(CalculatedTraits calculatedTraits);
    Characteristic[] CharacteristicsDependents();
}
