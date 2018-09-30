using System;

[Serializable]
public class MapEvent
{
    public string Type;
    public bool Visited;

    public virtual void RefreshEvent(CurrentEvent currentEvent)
    {
    }
}

[Serializable]
public class SpawnEvent : MapEvent
{
    public SpawnEvent()
    {
        Type = "Spawn";
    }
}

[Serializable]
public class EndMapEvent : MapEvent
{
    public EndMapEvent()
    {
        Type = "End";
    }

    public override void RefreshEvent(CurrentEvent currentEvent)
    {
        base.RefreshEvent(currentEvent);

        currentEvent.AccessNextMap();
    }
}