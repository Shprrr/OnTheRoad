using System;

[Serializable]
public class WeaponData : EquipableData
{
    public string AnimationNameAttack;

    public WeaponData()
    {
    }

    public WeaponData(WeaponData itemData, int amount) : base(itemData, amount)
    {
        AnimationNameAttack = itemData.AnimationNameAttack;
    }

    public WeaponData(string id, string name, int price, EquipmentSlot slot, string animationNameAttack, Trait[] traits, string description, int amount = 1) : base(id, name, price, slot, traits, description, amount)
    {
        AnimationNameAttack = animationNameAttack;
    }

    public override IItemData Copy(int amount)
    {
        return new WeaponData(this, amount);
    }
}
