using UnityEngine;

public class BattleStateAction : StateMachineBehaviour
{
    public BattleEvent battle;

    // Appelée sur la première image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        battle = (BattleEvent)animator.GetComponent<CurrentEvent>().currentEvent;

        if (battle.GetActiveBattler().IsPlayer)
        {
            Debug.LogFormat("CommandWindow for {0}", battle.GetActiveBattler().name);
            battle.RefreshButtons(true);
        }
        else
        {
            Debug.LogFormat("AIAction for {0}", battle.GetActiveBattler().name);
            battle._currentEvent.targetSelectionManager.ActionState(battle.GetActiveBattler(), battle.GetActiveBattler().GetComponent<BattleAI>().ChooseAction(battle.Enemies, battle.Actors));
        }
    }

    // Appelé à chaque image où Update est exécuté, sauf pour la première et la dernière image
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (battle.GetActiveBattler().CantFight)
        {
            // Active battler can't choose an action so we jump directly to the show damage state to apply changes in statuses.
            Debug.LogFormat("{0} Cant Fight", battle.GetActiveBattler().name);
            animator.SetInteger("state", 0);
            return;
        }
    }
}
