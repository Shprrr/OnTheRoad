using System.Linq;

public static class SkillFactory
{
    private static readonly SkillData[] datas = new SkillData[]
    {
        new SkillData("fire1", "Fire", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(7, 95), "A simple fire based spell."),
        new SkillData("multihit", "Multi-hit", 2, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, new DamageEffect(5, 95), "Creates an explosion that hits all enemies."),
        new SkillData("guard", "Guard", 1, "AnimationSpecial11", Cursor.POSSIBLE_TARGETS_SELF, new DamageEffect(5, 95), "Put itself in a defensive stance to reduce incoming attacks."),
        new SkillData("explosion", "Explosion", 1, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_ALL, new DamageEffect(20, 100), "Explosion itself to deal damage to everyone."),
        new SkillData("mm", "Magic Missile", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(7, 95), "A projectile of magical force."),
        new SkillData("spa", "Skill Points Attack", 3, "AnimationSpecial11", Cursor.POSSIBLE_TARGETS_ONE, new DamageEffect(5, 95, Damage.eDamageType.MP), "Attacks Skill Points instead of Hit Points.")
    };

    public static SkillData Build(string id)
    {
        return datas.Single(s => s.Id == id);
    }
}
