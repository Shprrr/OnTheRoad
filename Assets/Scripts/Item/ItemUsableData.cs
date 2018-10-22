﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using static Cursor;

[Serializable]
public class ItemUsableData : IItemData, IDataEffect, IEquatable<ItemUsableData>
{
    public string _id;
    public string _name;
    public string _description;
    public int _amount;
    public int _price;
    public string _animationName;
    public eTargetType[] _targetsPossible;
    public Effect _effect;
    public bool UsableOutsideBattle;

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public int Amount { get { return _amount; } set { _amount = value; } }
    public int Price { get { return _price; } set { _price = value; } }

#if UNITY_EDITOR
    [UnityEngine.HideInInspector]
    public bool foldout { get; set; }
#endif

    public string AnimationName { get { return _animationName; } set { _animationName = value; } }
    public eTargetType[] TargetsPossible { get { return _targetsPossible; } set { _targetsPossible = value; } }
    public Effect Effect { get { return _effect; } set { _effect = value; } }

    public ItemUsableData()
    {
    }

    public ItemUsableData(ItemUsableData itemData, int amount)
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

    public ItemUsableData(string id, string name, int price, string animationName, eTargetType[] targetsPossible, bool usableOutsideBattle, Effect effect, string description, int amount = 1)
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
        return Equals(obj as ItemUsableData);
    }

    public bool Equals(IItemData other)
    {
        return other != null &&
               Id == other.Id;
    }

    public bool Equals(ItemUsableData other)
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

    public IItemData Copy(int amount)
    {
        return new ItemUsableData(this, amount);
    }

    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ItemUsableData), typeof(Effect).Assembly.GetTypes().Where(t => typeof(Effect).IsAssignableFrom(t)).ToArray());
    public string Serialize()
    {
        using (var writer = new System.IO.StringWriter())
        {
            serializer.Serialize(writer, this);
            return writer.ToString();
        }
    }
    public IItemData Deserialize(string xml)
    {
        using (var reader = new System.IO.StringReader(xml))
        {
            return serializer.Deserialize(reader) as IItemData;
        }
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        var item = serializer.Deserialize(reader) as ItemUsableData;
        Id = item.Id;
        Name = item.Name;
        Description = item.Description;
        Amount = item.Amount;
        Price = item.Price;
        AnimationName = item.AnimationName;
        TargetsPossible = item.TargetsPossible;
        UsableOutsideBattle = item.UsableOutsideBattle;
        Effect = item.Effect;
    }

    public void WriteXml(XmlWriter writer)
    {
        serializer.Serialize(writer, this);
    }

    public static bool operator ==(ItemUsableData item1, ItemUsableData item2) => item1?.Id == item2?.Id;

    public static bool operator !=(ItemUsableData item1, ItemUsableData item2) => !(item1 == item2);
}
