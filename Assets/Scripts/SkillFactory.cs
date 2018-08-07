using System.Linq;

public static class SkillFactory
{
    private static readonly SkillData[] datas = new SkillData[]
    {
        new SkillData("Fire", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(7, 95)),
        new SkillData("Multi-hit", 2, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, new DamageEffect(5, 95)),
        new SkillData("Guard", 1, "AnimationSpecial11", Cursor.POSSIBLE_TARGETS_SELF, new DamageEffect(5, 95)),
        new SkillData("Explosion", 1, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_ALL, new DamageEffect(20, 100)),
        new SkillData("Magic Missile", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(7, 95))
    };

    public static SkillData Build(string name)
    {
        return datas.Single(s => s.Name == name);
    }
}
