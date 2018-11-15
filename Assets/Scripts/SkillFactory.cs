using System.Collections.Generic;
using System.Linq;

public static class SkillFactory
{
    private static readonly SkillData[] datas = new SkillData[0];

    static SkillFactory()
    {
        var list = new List<SkillData>();
        list.Add(new SkillData("fire1", "Fire", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, 3, new DamageMagicalEffect(5, 90), "A simple fire based spell."));
        list.Add(new SkillData("multihit", "Multi-hit", 2, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, 3, new DamagePhysicalEffect(5, false), "Creates an explosion that hits all enemies."));
        list.Add(new SkillData("guard", "Guard", 1, "AnimationSpecial11", Cursor.POSSIBLE_TARGETS_SELF, 5, new AddStatusEffect(new GuardStatus(), 100, 1, 1), "Put itself in a defensive stance to reduce incoming attacks."));
        list.Add(new SkillData("sd", "Self-destruct", 1, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_ALL, 3, new DamagePhysicalEffect(80, false), "Explosion itself to deal damage to everyone."));
        list.Add(new SkillData("mm", "Magic Missile", 4, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, 3, new DamageMagicalEffect(8, 90), "A projectile of magical force."));
        list.Add(new SkillData("spa", "Skill Points Attack", 3, "AnimationSpecial11", Cursor.POSSIBLE_TARGETS_ONE, 3, new DamagePhysicalEffect(5, false, Damage.eDamageType.MP), "Attacks Skill Points instead of Hit Points."));
        list.Add(new SkillData("explosion", "Explosion", 100, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, 6, new DamageMagicalEffect(80, 90), "My ultimate destructive magic... EXPLOSION !"));
        list.Add(new SkillData("meteor", "Meteor", 80, "AnimationAttack6", Cursor.POSSIBLE_TARGETS_MULTI, 6, new DamageMagicalEffect(80, 90), "Huge rock on fire falling from the sky."));
        list.Add(new SkillData("fire2", "Flame", 6, "AnimationFire1", Cursor.POSSIBLE_TARGETS_ONE, 4, new DamageMagicalEffect(5, 90), "An advanced fire based spell that can burn."));

        list.Add(new SkillData("buffStr", "Buff Strength", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, 3, new AddStatusEffect(new BuffStatus(BuffStatus.Stats.Strength, 1), 100), "Buff Strength from one level."));
        list.Add(new SkillData("buffVit", "Buff Vitality", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, 3, new AddStatusEffect(new BuffStatus(BuffStatus.Stats.Vitality, 1), 100), "Buff Vitality from one level."));
        list.Add(new SkillData("buffInt", "Buff Intellect", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, 3, new AddStatusEffect(new BuffStatus(BuffStatus.Stats.Intellect, 1), 20), "Buff Intellect from one level."));
        list.Add(new SkillData("buffWis", "Buff Wisdom", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, 3, new AddStatusEffect(new BuffStatus(BuffStatus.Stats.Wisdom, 1), 100), "Buff Wisdom from one level."));
        list.Add(new SkillData("buffAgi", "Buff Agility", 4, "AnimationHeal1", Cursor.POSSIBLE_TARGETS_ONE, 3, new AddStatusEffect(new BuffStatus(BuffStatus.Stats.Agility, 1), 100, 2, 3), "Buff Agility from one level."));

        datas = list.ToArray();
    }

    public static SkillData Build(string id)
    {
        return datas.Single(s => s.Id == id);
    }
}
