using UnityEngine;

public class ReadOnlyPropertyAttribute : PropertyAttribute
{
    public string MemberName;

    public ReadOnlyPropertyAttribute(string memberName)
    {
        MemberName = memberName;
    }
}
