using System.Linq;

public static class ItemFactory
{
    private static readonly ItemData[] datas = new ItemData[]
    {
        new ItemData("potionHp1", "Potion", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(7, 101), "Potion to regain health."),
        new ItemData("potionSp1", "Ether", 2, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101, Damage.eDamageType.MP), "Potion to regain skill points."),
        new ItemData("potionRevive1", "Revive", 1, "AnimationAttack1", Cursor.POSSIBLE_TARGETS_ONE, true, new HealEffect(5, 101), "A medicine that revives a fallen comrade."),
        new ItemData("molotov", "Molotov Cocktail", 1, "AnimationFire1", Cursor.POSSIBLE_TARGETS_MULTI, false, new DamageEffect(20, 101), "Throwing \"potion\" filled with a flammable liquid.")
    };

    public static ItemData Build(string id, int amount = 1)
    {
        return new ItemData(datas.Single(s => s.Id == id), amount);
    }
}
