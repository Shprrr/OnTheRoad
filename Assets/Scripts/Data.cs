using static Cursor;

public abstract class Data
{
    public string Id;
    public string Name;
    public string Description;
    public string AnimationName;
    public eTargetType[] TargetsPossible;
    public Effect Effect;

    public override string ToString()
    {
        return Name;
    }
}
