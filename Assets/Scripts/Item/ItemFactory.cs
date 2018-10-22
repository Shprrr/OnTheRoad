using System.Linq;

public static class ItemFactory
{
    private static readonly IItemData[] datas = new IItemData[]
    {
        new ItemUsableData("potionHp1", "Potion", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(7, 101), "Potion to regain health."),
        new ItemUsableData("potionSp1", "Ether", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101, Damage.eDamageType.MP), "Potion to regain skill points."),
        new ItemUsableData("potionRevive1", "Revive", 10, "AnimationAttack1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101), "A medicine that revives a fallen comrade."),
        new ItemUsableData("molotov", "Molotov Cocktail", 10, "AnimationFire1", Cursor.POSSIBLE_TARGETS_MULTI, false, new DamageMagicalEffect(20, 101), "Throwing \"potion\" filled with a flammable liquid."),
        new ItemUsableData("tknife", "Throwing Knife", 10, "AnimationSword1", Cursor.POSSIBLE_TARGETS_ONE, false, new DamagePhysicalEffect(10, true), "Knife specially designed and weighted so that it can be thrown effectively."),
        new ItemUsableData("apple", "Apple", 1, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101), "Good apple."),
        new ItemUsableData("tuna", "Tuna", 3, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(10, 101), "MAGURO FEVER.")
    };

    public static IItemData Build(string id, int amount = 1)
    {
        //return new ItemData(datas.Single(s => s.Id == id), amount);
        return datas.Single(s => s.Id == id).Copy(amount);
    }
}
