using System;

public interface IItemData : IData, IEquatable<IItemData>//, System.Xml.Serialization.IXmlSerializable
{
    int Amount { get; set; }
    int Price { get; set; }

    IItemData Copy(int amount);

#if UNITY_EDITOR
    [UnityEngine.HideInInspector]
    bool foldout { get; set; }
#endif

    string Serialize();
    IItemData Deserialize(string xml);
}
