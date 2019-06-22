using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CalculatorTrait
{
    private static Characteristic[] _characteristicsDefinitions;

    public static void UpdateDefinitions(Characteristic[] characteristicsDefinitions)
    {
        _characteristicsDefinitions = characteristicsDefinitions;
    }

    public static CalculatedTraits CalculateTraits(IEnumerable<Trait> traits)
    {
        var calcTraits = new CalculatedTraits();

        // Trie par dependances.
        var dependencySorted = _characteristicsDefinitions
            .Select(c => new CharacteristicDependant(c))
            .OrderBy(c => c.DependentsIdsToCalculate.Count).ToList();

        // Calcul ceux qui sont prêts (aucune dépendance non-calculé) récursivement
        while (dependencySorted.Count > 0)
        {
            calcTraits = CalculateOne(dependencySorted[0], traits, calcTraits);

            var id = dependencySorted[0].Id;
            dependencySorted.RemoveAt(0);
            foreach (var dependency in dependencySorted)
            {
                dependency.DependentsIdsToCalculate.Remove(id);
            }
        }

        return calcTraits;
    }

    private class CharacteristicDependant
    {
        public Characteristic Characteristic { get; private set; }
        public string Id => Characteristic.Id;
        public List<string> DependentsIdsToCalculate { get; private set; }
        public IBaseValueStrategy BaseValueStrategy => Characteristic.BaseValueStrategy;
        public IMinMaxStrategy MinMaxStrategy => Characteristic.MinMaxStrategy;

        public CharacteristicDependant(Characteristic characteristic)
        {
            Characteristic = characteristic ?? throw new ArgumentNullException(nameof(characteristic));
            var baseValueDependents = BaseValueStrategy.CharacteristicsDependents().Select(d => d.Id);
            var minMaxDependents = MinMaxStrategy.CharacteristicsDependents().Select(d => d.Id);
            DependentsIdsToCalculate = new List<string>(baseValueDependents.Union(minMaxDependents));
        }
    }

    private static CalculatedTraits CalculateOne(CharacteristicDependant characteristicToCalculate, IEnumerable<Trait> traits, CalculatedTraits calculatedTraits)
    {
        // Détermine base value.
        var value = characteristicToCalculate.BaseValueStrategy.GetBaseValue(calculatedTraits);

        // Prends tous les traits pour cette characteristic et regroupe par opération.
        var traitsToConsider = traits.Where(t => t.Characteristic.Id == characteristicToCalculate.Id).GroupBy(t => t.Operator).OrderBy(t => t.Key);

        // Calcul par groupe d'opération.
        foreach (var trait in traitsToConsider)
        {
            switch (trait.Key)
            {
                case TraitOperator.Addition:
                    value += trait.Sum(t => t.Value);
                    break;

                case TraitOperator.PercentAddition:
                    value *= 1 + trait.Sum(t => t.Value) / 100;
                    break;

                case TraitOperator.PercentMultiplication:
                    foreach (var t in trait)
                        value *= t.Value / 100;
                    break;
            }
        }

        calculatedTraits[characteristicToCalculate.Characteristic] = Mathf.Clamp(value, characteristicToCalculate.MinMaxStrategy.GetMinValue(calculatedTraits), characteristicToCalculate.MinMaxStrategy.GetMaxValue(calculatedTraits));

        return calculatedTraits;
    }
}
