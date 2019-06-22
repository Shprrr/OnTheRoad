using System.Collections.Generic;
using System.Linq;
using CF = CharacteristicFactory;
using ES = EquipableData.EquipmentSlot;
using TO = TraitOperator;

public static class ItemFactory
{
    private static readonly IItemData[] datas;

    static ItemFactory()
    {
#pragma warning disable IDE0028
        var list = new List<IItemData>(30 - 15 + 1);
        list.Add(new ItemUsableData("potionHp1", "Potion", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(10, 101), "Potion to regain health."));
        list.Add(new ItemUsableData("potionSp1", "Ether", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101, Damage.DamageType.MP), "Potion to regain skill points."));
        list.Add(new ItemUsableData("potionRevive1", "Revive", 10, "AnimationAttack1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101), "A medicine that revives a fallen comrade."));
        list.Add(new ItemUsableData("molotov", "Molotov Cocktail", 10, "AnimationFire1", Cursor.POSSIBLE_TARGETS_MULTI, false, new DamageMagicalEffect(20, 101), "Throwing \"potion\" filled with a flammable liquid."));
        list.Add(new ItemUsableData("tknife", "Throwing Knife", 10, "AnimationSword1", Cursor.POSSIBLE_TARGETS_ONE, false, new DamagePhysicalEffect(10, true), "Knife specially designed and weighted so that it can be thrown effectively."));
        list.Add(new ItemUsableData("apple", "Apple", 1, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101), "Good apple."));
        list.Add(new ItemUsableData("tuna", "Tuna", 3, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(10, 101), "MAGURO FEVER."));
        list.Add(new WeaponData("sword", "Sword", 100, ES.Weapon, "AnimationSword1", new Trait[] { new Trait(CF.PhysMinDamageId, 2, TO.Addition), new Trait(CF.PhysMaxDamageId, 5, TO.Addition), new Trait(CF.PhysHitRateId, 15, TO.Addition), new Trait(CF.PhysAttMultiplierId, 1, TO.Addition) }, "A sword."));
        list.Add(new WeaponData("staff", "Staff", 100, ES.Weapon, "AnimationAttack1", new Trait[] { new Trait(CF.PhysMinDamageId, 1, TO.Addition), new Trait(CF.PhysMaxDamageId, 3, TO.Addition), new Trait(CF.PhysHitRateId, 5, TO.Addition), new Trait(CF.MagMinDamageId, 2, TO.Addition), new Trait(CF.MagMaxDamageId, 5, TO.Addition), new Trait(CF.MagHitRateId, 15, TO.Addition), new Trait(CF.PhysAttMultiplierId, 1, TO.Addition) }, "A staff."));
        list.Add(new ArmorData("shield", "Shield", 50, ES.Offhand, new Trait[] { new Trait(CF.PhysDefenseId, 5, TO.Addition), new Trait(CF.PhysEvadeRateId, 10, TO.Addition), new Trait(CF.MagEvadeRateId, 10, TO.Addition), new Trait(CF.PhysDefMultiplierId, 1, TO.Addition), new Trait(CF.MagDefMultiplierId, 1, TO.Addition) }, "A shield."));
        list.Add(new ArmorData("helmet", "Helmet", 50, ES.Head, new Trait[] { new Trait(CF.PhysDefenseId, 5, TO.Addition), new Trait(CF.PhysEvadeRateId, 2, TO.Addition), new Trait(CF.MagDefenseId, -2, TO.Addition) }, "A helmet."));
        list.Add(new ArmorData("chestArmor", "Chest Armor", 100, ES.Body, new Trait[] { new Trait(CF.PhysDefenseId, 10, TO.Addition), new Trait(CF.PhysEvadeRateId, -1, TO.Addition), new Trait(CF.MagDefenseId, -5, TO.Addition), new Trait(CF.MagEvadeRateId, -1, TO.Addition) }, "A chest armor."));
        list.Add(new ArmorData("robe", "Robe", 100, ES.Body, new Trait[] { new Trait(CF.PhysEvadeRateId, 5, TO.Addition), new Trait(CF.MagDefenseId, 10, TO.Addition), new Trait(CF.MagEvadeRateId, 5, TO.Addition) }, "A robe."));
        list.Add(new ArmorData("boots", "Boots", 50, ES.Feet, new Trait[] { new Trait(CF.PhysDefenseId, 4, TO.Addition), new Trait(CF.PhysEvadeRateId, 3, TO.Addition), new Trait(CF.MagDefenseId, 2, TO.Addition), new Trait(CF.MagEvadeRateId, 3, TO.Addition) }, "Boots."));
        list.Add(new EquipableData("amulet", "Amulet", 150, ES.Neck, new Trait[] { }, "An amulet."));
        list.Add(new EquipableData("ring", "Ring", 100, ES.Finger, new Trait[] { new Trait(CF.MaxHPId, 10, TO.PercentAddition) }, "A ring.\n+10% Max HP"));
        list.Add(new ItemUsableData("sleepBomb", "Sleep Bomb", 100, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_MULTI, false, new AddStatusEffect(StatusTypeFactory.Build(StatusTypeFactory.SleepId), 100, 3, 5), "A bomb that puts targets asleep."));

        datas = list.ToArray();
    }

    public static IItemData Build(string id, int amount = 1)
    {
        //return new ItemData(datas.Single(s => s.Id == id), amount);
        return datas.Single(s => s.Id == id).Copy(amount);
    }
}
