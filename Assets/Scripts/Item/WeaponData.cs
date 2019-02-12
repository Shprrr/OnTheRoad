using System;

[Serializable]
public class WeaponData : EquipableData
{
    public string AnimationNameAttack;
    public int PhysicalMinDamage;
    public int PhysicalMaxDamage;
    public int PhysicalAccuracy;
    public int MagicalMinDamage;
    public int MagicalMaxDamage;
    public int MagicalAccuracy;

    public WeaponData()
    {
    }

    public WeaponData(WeaponData itemData, int amount) : base(itemData, amount)
    {
        AnimationNameAttack = itemData.AnimationNameAttack;
        PhysicalMinDamage = itemData.PhysicalMinDamage;
        PhysicalMaxDamage = itemData.PhysicalMaxDamage;
        PhysicalAccuracy = itemData.PhysicalAccuracy;
        MagicalMinDamage = itemData.MagicalMinDamage;
        MagicalMaxDamage = itemData.MagicalMaxDamage;
        MagicalAccuracy = itemData.MagicalAccuracy;
    }

    public WeaponData(string id, string name, int price, EquipmentSlot slot, string animationNameAttack, int physicalMinDamage, int physicalMaxDamage, int physicalAccuracy, int magicalMinDamage, int magicalMaxDamage, int magicalAccuracy, Trait[] traits, string description, int amount = 1) : base(id, name, price, slot, traits, description, amount)
    {
        AnimationNameAttack = animationNameAttack;
        PhysicalMinDamage = physicalMinDamage;
        PhysicalMaxDamage = physicalMaxDamage;
        PhysicalAccuracy = physicalAccuracy;
        MagicalMinDamage = magicalMinDamage;
        MagicalMaxDamage = magicalMaxDamage;
        MagicalAccuracy = magicalAccuracy;
    }

    public override IItemData Copy(int amount)
    {
        return new WeaponData(this, amount);
    }
}
