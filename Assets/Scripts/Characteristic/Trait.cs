using System;
using UnityEngine;

[Serializable]
public class Trait : ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [SerializeField, HideInInspector]
    private string Name;
#endif

    public Characteristic Characteristic;
    public float Value;
    public TraitOperator Operator;

    public Trait(string characteristicId, float value, TraitOperator @operator)
    {
        Characteristic = CharacteristicFactory.Build(characteristicId);
        Value = value;
        Operator = @operator;
#if UNITY_EDITOR
        Name = ToString();
#endif
    }

    public override string ToString()
    {
        return Characteristic + " " + Operator + " " + Value;
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
#if UNITY_EDITOR
        Name = ToString();
#endif
    }
}

public enum TraitOperator
{
    Addition,
    PercentAddition,
    PercentMultiplication
}
