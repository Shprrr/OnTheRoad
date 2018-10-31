using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTBManager : MonoBehaviour
{
    [Serializable]
    public struct CTBTurn : IComparable<CTBTurn>
    {
        public const int RANK_DEFAULT = 3;
        public const int RANK_ITEM = 2;

        public int counter;
        public int rank;
        public int tickSpeed;
        public Battler battler;

        public void SetCounter()
        {
            counter += (int)(tickSpeed * rank * battler.HasteStatus);
            rank = RANK_DEFAULT;
        }

        public override int GetHashCode() { return base.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj is CTBTurn)
            {
                CTBTurn other = (CTBTurn)obj;
                if (battler == other.battler)
                    return counter == other.counter;

                return false;
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return battler + " [C:" + counter + "]";
        }

        #region IComparable<CTBTurn> Membres

        public int CompareTo(CTBTurn other)
        {
            if (other.battler == null)
                return -1;

            if (battler == null)
                return 1;

            return counter.CompareTo(other.counter);
        }

        #endregion
    }

    private int MAX_CTB = 16;

    public GameObject ctbContent;
    public GameObject prefabCTBPortrait;

    public int BattleTurn = 0;
    public List<CTBTurn> OrderBattle;
    private int _CounterActiveTurn;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        OrderBattle = new List<CTBTurn>(MAX_CTB);
    }

    private void RefreshCTB()
    {
        ctbContent.DestroyAllChildren();

        foreach (var turn in OrderBattle)
        {
            var portrait = Instantiate(prefabCTBPortrait, ctbContent.transform);
            portrait.GetComponent<Image>().sprite = turn.battler.GetComponentInChildren<Image>().sprite;
            portrait.GetComponentInChildren<Text>().text = "";
        }
    }

    public void StartBattle()
    {
        BattleTurn = 0;
        OrderBattle.Clear();
    }

    public void CalculateCTB(BattleEvent battle)
    {
        CTBTurn? firstTurn = null;
        if (BattleTurn > 0)
        {
            firstTurn = OrderBattle[0];
            // Empty the OrderBattle to recalculate TickSpeed changes and Dead/Alive changes.
            OrderBattle.Clear();
        }

        //1-Get next CV as ICV, ajoute le plus petit et garde les restes
        List<CTBTurn> tempCTB = new List<CTBTurn>(battle.MAX_ACTOR + battle.MAX_ENEMY);

        //Get ICVs
        for (int i = 0; i < battle.Actors.Count; i++)
        {
            if (battle.Actors[i].IsDead)
                continue;

            if (BattleTurn == 0)
                battle.Actors[i].CalculateICV();
            if (firstTurn.HasValue && battle.Actors[i] == firstTurn.Value.battler)
                tempCTB.Add(firstTurn.Value);
            else
            {
                CTBTurn turn = new CTBTurn();
                turn.battler = battle.Actors[i];
                turn.rank = CTBTurn.RANK_DEFAULT;
                turn.counter = turn.battler.CounterCTB;
                turn.tickSpeed = turn.battler.getTickSpeed();
                tempCTB.Add(turn);
            }
        }

        for (int i = 0; i < battle.Enemies.Count; i++)
        {
            if (battle.Enemies[i].IsDead)
                continue;

            if (BattleTurn == 0)
                battle.Enemies[i].CalculateICV();
            if (firstTurn.HasValue && battle.Enemies[i] == firstTurn.Value.battler)
                tempCTB.Add(firstTurn.Value);
            else
            {
                CTBTurn turn = new CTBTurn();
                turn.battler = battle.Enemies[i];
                turn.rank = CTBTurn.RANK_DEFAULT;
                turn.counter = turn.battler.CounterCTB;
                turn.tickSpeed = turn.battler.getTickSpeed();
                tempCTB.Add(turn);
            }
        }

        //Sort ICVs
        tempCTB.Sort();

        //Keep lowest
        OrderBattle.Add(tempCTB[0]);

        //2-Calcul le NCV de celui ajouté, ajoute le plus petit et garde les restes
        for (int i = 1; i < MAX_CTB; i++)
        {
            //Get Next CV
            CTBTurn turn = tempCTB[0];
            turn.SetCounter();
            tempCTB[0] = turn;

            //Sort CVs
            tempCTB.Sort();

            //Keep lowest
            OrderBattle.Add(tempCTB[0]);
        }

        setActiveBattler(battle);
        RefreshCTB();
    }

    private void setActiveBattler(BattleEvent battle)
    {
        CTBTurn activeTurn = OrderBattle[0];
        for (int i = 0; i < OrderBattle.Count; i++)
        {
            if (!OrderBattle[i].battler.CantFight)
            {
                activeTurn = OrderBattle[i];
                _CounterActiveTurn = activeTurn.counter;
                break;
            }
        }

        for (int i = 0; i < battle.Actors.Count; i++)
        {
            if (battle.Actors[i].IsDead)
                continue;

            if (!battle.Actors[i].CantFight)
                battle.Actors[i].CounterCTB -= _CounterActiveTurn;
            if (battle.Actors[i] == activeTurn.battler)
                battle.ActiveBattlerIndex = i;
        }

        for (int i = 0; i < battle.Enemies.Count; i++)
        {
            if (battle.Enemies[i].IsDead)
                continue;

            if (!battle.Enemies[i].CantFight)
                battle.Enemies[i].CounterCTB -= _CounterActiveTurn;
            if (battle.Enemies[i] == activeTurn.battler)
                battle.ActiveBattlerIndex = battle.MAX_ACTOR + i;
        }

        // Remove active counter on all other counters.
        for (int i = 0; i < OrderBattle.Count; i++)
        {
            if (OrderBattle[i].battler.CantFight)
                continue;

            CTBTurn turn = OrderBattle[i];
            turn.counter -= _CounterActiveTurn;
            OrderBattle[i] = turn;
        }

        OrderBattle.Sort(); // Some battlers can't move so this could change the order.
    }

    public void BeginTurn(BattleEvent battle)
    {
        // Keep the CV of the next turn of the current battler
        if (BattleTurn > 0)
        {
            CTBTurn turn = OrderBattle[0];
            turn.SetCounter();
            OrderBattle[0] = turn;
            OrderBattle[0].battler.CounterCTB = OrderBattle[0].counter;
        }

        // Update CTB and change the active battler for the next one.
        CalculateCTB(battle);

        BattleTurn++;
    }

    /// <summary>
    /// Change the rank of the turn for the active battler.
    /// </summary>
    /// <param name="rank">rank of this turn</param>
    public void ChangeActiveRank(BattleEvent battle, int rank)
    {
        var firstTurn = OrderBattle[0];
        firstTurn.rank = rank;
        OrderBattle[0] = firstTurn;
        CalculateCTB(battle);
    }
}
