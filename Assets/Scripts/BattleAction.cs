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
    //public BaseItem Item;

    //public BattleAction(eKind kind = eKind.WAIT, Skill skill = null, BaseItem item = null)
    //{
    //    this.Kind = kind;
    //    this.Target = null;
    //    this.Skill = skill;
    //    this.Item = item;
    //}

    public BattleAction(BattleCommand kind = BattleCommand.Nothing, SkillData skill = null)
    {
        Kind = kind;
        Target = null;
        Skill = skill;
    }
}
