using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlerStatus : MonoBehaviour
{
    public List<Status> ActiveStatuses = new List<Status>();

    public bool AddStatus(Status status)
    {
        var activeStatusesByType = ActiveStatuses.ToDictionary(s => s.Type);
        if (activeStatusesByType.TryGetValue(status.Type, out var activeStatus))
            return activeStatus.StackStatus(status);

        ActiveStatuses.Add(status);
        return true;
    }

    public bool RemoveStatus(StatusType[] type)
    {
        var removed = false;
        for (int i = ActiveStatuses.Count - 1; i >= 0; i--)
        {
            if (type.Any(t => t == ActiveStatuses[i].Type))
            {
                ActiveStatuses.RemoveAt(i);
                removed = true;
            }
        }

        return removed;
    }

    public void PassTurn()
    {
        for (int i = ActiveStatuses.Count - 1; i >= 0; i--)
        {
            if (ActiveStatuses[i].TurnLeft > 0)
                ActiveStatuses[i].TurnLeft--;
            if (ActiveStatuses[i].TurnLeft == 0)
                ActiveStatuses.RemoveAt(i);
        }
    }

    public bool IsAlive()
    {
        return ActiveStatuses.All(s => s.Type.IsAlive);
    }

    public RestrictionType? GetRestriction()
    {
        return ActiveStatuses.Min(s => s.Type.Restriction);
    }
}
