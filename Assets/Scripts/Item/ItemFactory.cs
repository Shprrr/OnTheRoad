using System.Collections.Generic;
using System.Linq;
using static SecondaryParameters;

public static class ItemFactory
{
    private static readonly IItemData[] datas;

    static ItemFactory()
    {
        var list = new List<IItemData>(27 - 12 + 1);
        list.Add(new ItemUsableData("potionHp1", "Potion", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(10, 101), "Potion to regain health."));
        list.Add(new ItemUsableData("potionSp1", "Ether", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101, Damage.eDamageType.MP), "Potion to regain skill points."));
        list.Add(new ItemUsableData("potionRevive1", "Revive", 10, "AnimationAttack1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101), "A medicine that revives a fallen comrade."));
        list.Add(new ItemUsableData("molotov", "Molotov Cocktail", 10, "AnimationFire1", Cursor.POSSIBLE_TARGETS_MULTI, false, new DamageMagicalEffect(20, 101), "Throwing \"potion\" filled with a flammable liquid."));
        list.Add(new ItemUsableData("tknife", "Throwing Knife", 10, "AnimationSword1", Cursor.POSSIBLE_TARGETS_ONE, false, new DamagePhysicalEffect(10, true), "Knife specially designed and weighted so that it can be thrown effectively."));
        list.Add(new ItemUsableData("apple", "Apple", 1, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101), "Good apple."));
        list.Add(new ItemUsableData("tuna", "Tuna", 3, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(10, 101), "MAGURO FEVER."));
        list.Add(new WeaponData("sword", "Sword", 100, EquipableData.EquipmentSlot.Weapon, "AnimationSword1", 2, 5, 95, 0, 0, 0, new Trait[] { Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalMinDamage, 1), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalMaxDamage, 4), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalHitRate, 15) }, "A sword."));
        list.Add(new WeaponData("staff", "Staff", 100, EquipableData.EquipmentSlot.Weapon, "AnimationAttack1", 1, 3, 95, 2, 5, 5, new Trait[] { Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalMinDamage, 0), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalMaxDamage, 2), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalHitRate, 15) }, "A staff."));
        list.Add(new ArmorData("shield", "Shield", 50, EquipableData.EquipmentSlot.Offhand, 5, 10, 0, 10, new Trait[] { Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalDefense, 5), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalEvadeRate, 10) }, "A shield."));
        list.Add(new ArmorData("helmet", "Helmet", 50, EquipableData.EquipmentSlot.Head, 5, 2, -2, 0, new Trait[] { Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalDefense, 5), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalEvadeRate, 2) }, "A helmet."));
        list.Add(new ArmorData("chestArmor", "Chest Armor", 100, EquipableData.EquipmentSlot.Body, 10, -1, -5, -1, new Trait[] { Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalDefense, 10), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalEvadeRate, -1) }, "A chest armor."));
        list.Add(new ArmorData("robe", "Robe", 100, EquipableData.EquipmentSlot.Body, 0, 5, 10, 5, new Trait[] { Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalEvadeRate, 5) }, "A robe."));
        list.Add(new ArmorData("boots", "Boots", 50, EquipableData.EquipmentSlot.Feet, 4, 3, 2, 3, new Trait[] { Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalDefense, 4), Trait.BuildSecondaryParamTrait(SecondaryParameterIndex.PhysicalEvadeRate, 3) }, "Boots."));
        list.Add(new EquipableData("amulet", "Amulet", 150, EquipableData.EquipmentSlot.Neck, new Trait[] { }, "An amulet."));
        list.Add(new EquipableData("ring", "Ring", 100, EquipableData.EquipmentSlot.Finger, new Trait[] { }, "A ring."));

        datas = list.ToArray();
    }

    public static IItemData Build(string id, int amount = 1)
    {
        //return new ItemData(datas.Single(s => s.Id == id), amount);
        return datas.Single(s => s.Id == id).Copy(amount);
    }
}
