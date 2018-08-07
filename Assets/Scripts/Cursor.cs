using System.Collections.Generic;
using System.Linq;

public class Cursor
{
    public enum eTargetType
    {
        NONE,
        SINGLE_ENEMY,
        MULTI_ENEMY,
        SINGLE_PARTY,
        MULTI_PARTY,
        SELF,
        ALL
    }

    public static readonly eTargetType[] POSSIBLE_TARGETS_ANYONE = new eTargetType[] {
            eTargetType.SINGLE_ENEMY,
            eTargetType.MULTI_ENEMY,
            eTargetType.SINGLE_PARTY,
            eTargetType.MULTI_PARTY,
            eTargetType.ALL };

    public static readonly eTargetType[] POSSIBLE_TARGETS_ALL = new eTargetType[] {
            eTargetType.ALL };

    public static readonly eTargetType[] POSSIBLE_TARGETS_MULTI = new eTargetType[] {
            eTargetType.MULTI_ENEMY,
            eTargetType.MULTI_PARTY };

    public static readonly eTargetType[] POSSIBLE_TARGETS_ONE = new eTargetType[] {
            eTargetType.SINGLE_ENEMY,
            eTargetType.SINGLE_PARTY };

    public static readonly eTargetType[] POSSIBLE_TARGETS_SELF = new eTargetType[] {
            eTargetType.SELF };

    public List<Battler> Actors { get; private set; }
    public List<Battler> Enemies { get; private set; }
    public int Index { get; set; }
    public Battler SingleTarget { get; set; }
    //public int IndexSelf { get; set; }
    public Battler Self { get; set; }
    public eTargetType TargetType { get; set; }
    public eTargetType[] PossibleTargets { get; set; }

    public Cursor(eTargetType defaultTarget, Battler self, eTargetType[] possibleTargets, List<Battler> actors, List<Battler> enemies)
    {
        Actors = actors ?? new List<Battler>();
        Enemies = enemies ?? new List<Battler>();
        Index = 0;
        //IndexSelf = indexSelf;
        Self = self;
        TargetType = defaultTarget;
        PossibleTargets = possibleTargets;
    }

    private static readonly eTargetType[] TARGET_ORDER = new eTargetType[] {
            eTargetType.ALL,
            eTargetType.MULTI_ENEMY,
            eTargetType.SINGLE_ENEMY,
            eTargetType.SINGLE_PARTY,
            eTargetType.MULTI_PARTY };

    public void ChangeTargetTypeToLeft()
    {
        if (!TARGET_ORDER.Contains(TargetType))
            return;

        int i;
        for (i = 0; i < TARGET_ORDER.Length; i++)
        {
            if (TargetType == TARGET_ORDER[i])
                break;
        }

        // Go to the previous Possible Target and loop if needed.
        do
        {
            i--;
            if (i < 0) i = TARGET_ORDER.Length - 1;
            TargetType = TARGET_ORDER[i];
        } while (!PossibleTargets.Contains(TargetType));

        while (TargetType == eTargetType.SINGLE_PARTY && Actors[Index].IsDead)
            GoToNextActor();

        while (TargetType == eTargetType.SINGLE_ENEMY && Enemies[Index].IsDead)
            GoToNextEnemy();
    }

    public void ChangeTargetTypeToRight()
    {
        if (!TARGET_ORDER.Contains(TargetType))
            return;

        int i;
        for (i = 0; i < TARGET_ORDER.Length; i++)
        {
            if (TargetType == TARGET_ORDER[i])
                break;
        }

        // Go to the next Possible Target and loop if needed.
        do
        {
            i++;
            if (i >= TARGET_ORDER.Length) i = 0;
            TargetType = TARGET_ORDER[i];
        } while (!PossibleTargets.Contains(TargetType));

        while (TargetType == eTargetType.SINGLE_PARTY && Actors[Index].IsDead)
            GoToNextActor();

        while (TargetType == eTargetType.SINGLE_ENEMY && Enemies[Index].IsDead)
            GoToNextEnemy();
    }

    public bool ChangeCursorDown()
    {
        // If we can go down.
        if (TargetType == eTargetType.SINGLE_PARTY)
        {
            GoToNextActor();
            return true;
        }
        else if (TargetType == eTargetType.SINGLE_ENEMY)
        {
            GoToNextEnemy();
            return true;
        }
        return false;
    }

    public bool ChangeCursorUp()
    {
        // If we can go up.
        if (TargetType == eTargetType.SINGLE_PARTY)
        {
            GoToPreviousActor();
            return true;
        }
        else if (TargetType == eTargetType.SINGLE_ENEMY)
        {
            GoToPreviousEnemy();
            return true;
        }
        return false;
    }

    private void GoToPreviousActor()
    {
        do
        {
            if (Index > 0)
                Index--;
            else
                Index = Actors.Count - 1;
        }
        while (Actors[Index] == null);
    }

    private void GoToPreviousEnemy()
    {
        do
        {
            if (Index > 0)
                Index--;
            else
                Index = Enemies.Count - 1;
        }
        while (Enemies[Index] == null);
    }

    private void GoToNextActor()
    {
        do
        {
            if (Index < Actors.Count - 1)
                Index++;
            else
                Index = 0;
        }
        while (Actors[Index] == null);
    }

    private void GoToNextEnemy()
    {
        do
        {
            if (Index < Enemies.Count - 1)
                Index++;
            else
                Index = 0;
        }
        while (Enemies[Index] == null);
    }

    public List<Battler> getTargetBattler()
    {
        var targets = new List<Battler>(Actors.Count + Enemies.Count);
        switch (TargetType)
        {
            case eTargetType.SINGLE_ENEMY:
                //targets.Add(Enemies[Index]);
                targets.Add(SingleTarget);
                break;

            case eTargetType.MULTI_ENEMY:
                targets.AddRange(Enemies);
                break;

            case eTargetType.SINGLE_PARTY:
                //targets.Add(Actors[Index]);
                targets.Add(SingleTarget);
                break;

            case eTargetType.MULTI_PARTY:
                targets.AddRange(Actors);
                break;

            case eTargetType.SELF:
                //if (IndexSelf < Battle.MAX_ACTOR)
                //    targets.Add(Actors[IndexSelf]);
                //else
                //    targets.Add(Enemies[IndexSelf - Battle.MAX_ACTOR]);
                targets.Add(Self);
                break;

            case eTargetType.ALL:
                targets.AddRange(Actors);
                targets.AddRange(Enemies);
                break;
        }
        return targets;
    }
}
