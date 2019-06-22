using System;

[Serializable]
public class ArmorData : EquipableData
{
    public ArmorData()
    {
    }

    public ArmorData(ArmorData itemData, int amount) : base(itemData, amount)
    {
    }

    public ArmorData(string id, string name, int price, EquipmentSlot slot, Trait[] traits, string description, int amount = 1) : base(id, name, price, slot, traits, description, amount)
    {
    }

    public override IItemData Copy(int amount)
    {
        return new ArmorData(this, amount);
    }
}
