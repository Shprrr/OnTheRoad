using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlerStatus : MonoBehaviour
{
    public List<Status> ActiveStatuses = new List<Status>();

    public bool AddStatus(Status status)
    {
        var activeStatus = ActiveStatuses.SingleOrDefault(s => s.Id == status.Id);
        if (activeStatus != null)
            return activeStatus.Stackable(status);

        ActiveStatuses.Add(status);
        return true;
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

    public bool HasStatus<T>() where T : Status
    {
        return ActiveStatuses.Any(s => s is T);
    }

    public bool HasStatus<T>(Func<T, bool> predicate) where T : Status
    {
        return ActiveStatuses.Any(s => s is T && predicate.Invoke((T)s));
    }

    public T GetStatus<T>() where T : Status
    {
        return (T)ActiveStatuses.SingleOrDefault(s => s is T);
    }

    public T GetStatus<T>(Func<T, bool> predicate) where T : Status
    {
        return (T)ActiveStatuses.SingleOrDefault(s => s is T && predicate.Invoke((T)s));
    }
}
