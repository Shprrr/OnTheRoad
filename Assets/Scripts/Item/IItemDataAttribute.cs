using UnityEngine;

public class IItemDataAttribute : PropertyAttribute
{
    public string MemberName;

    public IItemDataAttribute(string memberName)
    {
        MemberName = memberName;
    }
}
