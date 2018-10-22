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
    public IDataEffect Data;

    public BattleAction(BattleCommand kind = BattleCommand.Nothing, IDataEffect data = null, Cursor target = null)
    {
        Kind = kind;
        Target = target;
        Data = data;
    }

    public override string ToString()
    {
        string r = Kind.ToString();
        if (Kind == BattleCommand.Skills || Kind == BattleCommand.Items)
            r = Data.ToString();

        if (Target != null)
            r += " on " + Target;

        return r;
    }
}
