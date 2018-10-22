using System.Linq;

public static class SkillFactory
{
    private static readonly SkillData[] datas = new SkillData[]
    {
        new SkillData("fire1", "Fire", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, new DamageMagicalEffect(7, 95), "A simple fire based spell."),
        new SkillData("multihit", "Multi-hit", 2, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, new DamagePhysicalEffect(5, false), "Creates an explosion that hits all enemies."),
        new SkillData("guard", "Guard", 1, "AnimationSpecial11", Cursor.POSSIBLE_TARGETS_SELF, new DamagePhysicalEffect(5, false), "Put itself in a defensive stance to reduce incoming attacks."),
        new SkillData("sd", "Self-destruct", 1, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_ALL, new DamagePhysicalEffect(80, false), "Explosion itself to deal damage to everyone."),
        new SkillData("mm", "Magic Missile", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, new DamageMagicalEffect(8, 95), "A projectile of magical force."),
        new SkillData("spa", "Skill Points Attack", 3, "AnimationSpecial11", Cursor.POSSIBLE_TARGETS_ONE, new DamagePhysicalEffect(5, false, Damage.eDamageType.MP), "Attacks Skill Points instead of Hit Points."),
        new SkillData("explosion", "Explosion", 100, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, new DamageMagicalEffect(80, 95), "My ultimate destructive magic... EXPLOSION !"),
        new SkillData("meteor", "Meteor", 80, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, new DamageMagicalEffect(80, 95), "Huge rock on fire falling from the sky.")
    };

    public static SkillData Build(string id)
    {
        return datas.Single(s => s.Id == id);
    }
}
