using System;

[Serializable]
public class MapEvent
{
    public string Type;

    public virtual void RefreshEvent(CurrentEvent currentEvent)
    {
    }
}