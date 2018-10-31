using System;
using System.Collections.Generic;

[Serializable]
public class EquipableData : IItemData, IEquatable<EquipableData>
{
    public string _id;
    public string _name;
    public string _description;
    public int _amount;
    public int _price;

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public int Amount { get { return _amount; } set { _amount = value; } }
    public int Price { get { return _price; } set { _price = value; } }

#if UNITY_EDITOR
    [UnityEngine.HideInInspector]
    public bool foldout { get; set; }
#endif


    public enum EquipmentSlot
    {
        Weapon,
        Offhand,
        Head,
        Body,
        Feet,
        Neck,
        Finger
    }

    public EquipmentSlot Slot;

    public EquipableData()
    {
    }

    public EquipableData(EquipableData itemData, int amount)
    {
        Id = itemData.Id;
        Name = itemData.Name;
        Description = itemData.Description;
        Amount = amount;
        Price = itemData.Price;
        Slot = itemData.Slot;
    }

    public EquipableData(string id, string name, int price, EquipmentSlot slot, string description, int amount = 1)
    {
        Id = id;
        Name = name;
        Description = description;
        Amount = amount;
        Price = price;
        Slot = slot;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as EquipableData);
    }

    public bool Equals(IItemData other)
    {
        return other != null &&
               Id == other.Id;
    }

    public bool Equals(EquipableData other)
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

    public virtual IItemData Copy(int amount)
    {
        return new EquipableData(this, amount);
    }

    public static bool operator ==(EquipableData item1, EquipableData item2) => item1?.Id == item2?.Id;

    public static bool operator !=(EquipableData item1, EquipableData item2) => !(item1 == item2);
}
