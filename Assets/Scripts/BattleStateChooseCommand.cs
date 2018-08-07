using UnityEngine;

public class BattleStateChooseCommand : StateMachineBehaviour
{
    public BattleEvent battle;

    // Appelée sur la première image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        battle = (BattleEvent)animator.GetComponent<CurrentEvent>().currentEvent;

        // Determine win/loss situation
        if (battle.Judge())
        {
            // If won or lost: end method
            animator.SetBool("end", true);
            return;
        }

        battle.ctbManager.BeginTurn(battle);

        //TODO: Statuses
        //foreach (var actor in Actors)
        //{
        //    if (!Character.IsNullOrDead(actor))
        //        foreach (var status in actor.Statuses)
        //        {
        //            status.Value.OnBeginTurn(actor, _CounterActiveTurn);
        //        }
        //}

        //foreach (var enemy in Enemies)
        //{
        //    if (!Character.IsNullOrDead(enemy))
        //        foreach (var status in enemy.Statuses)
        //        {
        //            status.Value.OnBeginTurn(enemy, _CounterActiveTurn);
        //        }
        //}

        if (!battle.getActiveBattler().CantFight)
            animator.SetInteger("state", 1);
        else
        {
            // Active battler can't choose an action so we jump directly to the show damage state to apply changes in statuses.
            Debug.LogFormat("DoNothing for {0}", battle.getActiveBattler().name);
            animator.SetInteger("state", 2);
        }
    }
}
