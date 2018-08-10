public struct BattleAction
{
    public const int RANK_ATTACK = 3;
    public const int RANK_ITEM = 2;
    public const int RANK_DEFEND = 2;
    public const int RANK_ESCAPE = 1;

    public enum BattleCommand
    {
        Nothing,
        Attack,
        Skills,
        Items,
        Run
    }

    public BattleCommand Kind;
    public Cursor Target;
    public SkillData Skill;
    public ItemData Item;

    public BattleAction(BattleCommand kind = BattleCommand.Nothing, SkillData skill = null, ItemData item = null)
    {
        Kind = kind;
        Target = null;
        Skill = skill;
        Item = item;
    }

    public override string ToString()
    {
        string r = Kind.ToString();
        if (Kind == BattleCommand.Skills)
            r = Skill.ToString();
        if (Kind == BattleCommand.Items)
            r = Item.ToString();

        if (Target != null)
            r += " on " + Target;

        return r;
    }
}
