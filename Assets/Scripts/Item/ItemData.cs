using System;
using System.Collections.Generic;
using static Cursor;

[Obsolete]
[Serializable]
public class ItemData : IDataEffect, IEquatable<ItemData>
{
    public int Amount;
    public int Price;

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
    }

    public ItemData(string id, string name, int price, string description, int amount = 1)
    {
        Id = id;
        Name = name;
        Description = description;
        Amount = amount;
        Price = price;
    }

    public bool UsableOutsideBattle;

    public string AnimationName
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
    public eTargetType[] TargetsPossible
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
    public Effect Effect
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
    public string Id
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
    public string Name
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
    public string Description
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public ItemData(string id, string name, int price, string animationName, eTargetType[] targetsPossible, bool usableOutsideBattle, Effect effect, string description, int amount = 1) : this(id, name, price, description, amount)
    {
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
