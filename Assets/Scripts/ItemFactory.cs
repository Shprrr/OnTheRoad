using System.Linq;

public class ItemFactory
{
    private static readonly ItemData[] datas = new ItemData[]
    {
        new ItemData("Potion", 4, "AnimationAttack1", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(7, 95)),
        new ItemData("Ether", 2, "AnimationAttack1", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(5, 95)),
        new ItemData("Revive", 1, "AnimationAttack1", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(5, 95)),
        new ItemData("Cocktail Molotov", 1, "AnimationFire1", Cursor.POSSIBLE_TARGETS_MULTI, new DamageEffect(20, 100))
    };

    public static ItemData Build(string name)
    {
        return datas.Single(s => s.Name == name);
    }
}
