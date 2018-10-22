using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

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

    public IItemData Copy(int amount)
    {
        return new EquipableData(this, amount);
    }

    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(EquipableData), typeof(Effect).Assembly.GetTypes().Where(t => typeof(Effect).IsAssignableFrom(t)).ToArray());
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
        var item = serializer.Deserialize(reader) as EquipableData;
        Id = item.Id;
        Name = item.Name;
        Description = item.Description;
        Amount = item.Amount;
        Price = item.Price;
    }

    public void WriteXml(XmlWriter writer)
    {
        serializer.Serialize(writer, this);
    }

    public static bool operator ==(EquipableData item1, EquipableData item2) => item1?.Id == item2?.Id;

    public static bool operator !=(EquipableData item1, EquipableData item2) => !(item1 == item2);
}
