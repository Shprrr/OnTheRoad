using static Cursor;

public interface IDataEffect : IData
{
    string AnimationName { get; set; }
    eTargetType[] TargetsPossible { get; set; }
    Effect Effect { get; set; }
}
