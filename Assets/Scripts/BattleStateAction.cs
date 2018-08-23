using UnityEngine;

public class BattleStateAction : StateMachineBehaviour
{
    public BattleEvent battle;

    // Appelée sur la première image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        battle = (BattleEvent)animator.GetComponent<CurrentEvent>().currentEvent;

        if (battle.getActiveBattler().IsPlayer)
        {
            Debug.LogFormat("CommandWindow for {0}", battle.getActiveBattler().name);
            battle.RefreshButtons(true);
        }
        else
        {
            Debug.LogFormat("AIAction for {0}", battle.getActiveBattler().name);
            battle._currentEvent.targetSelectionManager.ActionState(battle.getActiveBattler(), battle.getActiveBattler().GetComponent<BattleAI>().ChooseAction(battle.Enemies, battle.Actors));
        }
    }
}
