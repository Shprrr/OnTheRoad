using System;
using System.Collections.Generic;
using static Cursor;

[Serializable]
public class ItemData : DataEffect, IEquatable<ItemData>
{
    public int Amount;
    public int Price;
    public bool UsableOutsideBattle;

    public ItemData()
    {
    }

    public ItemData(ItemData itemData, int amount)
    {
        Id = itemData.Id;
        Name = itemData.Name;
        Description = itemData.Description;
        Amount = amount;
        Price = itemData.Price;
        AnimationName = itemData.AnimationName;
        TargetsPossible = itemData.TargetsPossible;
        UsableOutsideBattle = itemData.UsableOutsideBattle;
        Effect = itemData.Effect;
    }

    public ItemData(string id, string name, int price, string animationName, eTargetType[] targetsPossible, bool usableOutsideBattle, Effect effect, string description, int amount = 1)
    {
        Id = id;
        Name = name;
        Description = description;
        Amount = amount;
        Price = price;
        AnimationName = animationName;
        TargetsPossible = targetsPossible;
        UsableOutsideBattle = usableOutsideBattle;
        Effect = effect;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ItemData);
    }

    public bool Equals(ItemData other)
    {
        return other != null &&
               Id == other.Id;
    }

    public override int GetHashCode()
    {
        return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
    }

    public override string ToString()
    {
        return Name;
    }

    public static bool operator ==(ItemData item1, ItemData item2) => item1?.Id == item2?.Id;

    public static bool operator !=(ItemData item1, ItemData item2) => !(item1 == item2);
}
