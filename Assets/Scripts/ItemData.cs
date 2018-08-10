using System;
using static Cursor;

[Serializable]
public class ItemData
{
    public string Name;
    public int Amount;
    public int Price;
    public string AnimationName;
    public eTargetType[] TargetsPossible;
    public Effect Effect;

    public ItemData()
    {
    }

    public ItemData(string name, int price, string animationName, eTargetType[] targetsPossible, Effect effect, int amount = 1)
    {
        Name = name;
        Amount = amount;
        Price = price;
        AnimationName = animationName;
        TargetsPossible = targetsPossible;
        Effect = effect;
    }

    public override string ToString()
    {
        return Name;
    }
}
