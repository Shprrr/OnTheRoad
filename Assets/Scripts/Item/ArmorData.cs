using System;

[Serializable]
public class ArmorData : EquipableData
{
    public int PhysicalArmor;
    public int PhysicalEvasion;
    public int MagicalArmor;
    public int MagicalEvasion;

    public ArmorData()
    {
    }

    public ArmorData(ArmorData itemData, int amount) : base(itemData, amount)
    {
        PhysicalArmor = itemData.PhysicalArmor;
        PhysicalEvasion = itemData.PhysicalEvasion;
        MagicalArmor = itemData.MagicalArmor;
        MagicalEvasion = itemData.MagicalEvasion;
    }

    public ArmorData(string id, string name, int price, EquipmentSlot slot, int physicalArmor, int physicalEvasion, int magicalArmor, int magicalEvasion, string description, int amount = 1) : base(id, name, price, slot, description, amount)
    {
        PhysicalArmor = physicalArmor;
        PhysicalEvasion = physicalEvasion;
        MagicalArmor = magicalArmor;
        MagicalEvasion = magicalEvasion;
    }

    public override IItemData Copy(int amount)
    {
        return new ArmorData(this, amount);
    }
}
