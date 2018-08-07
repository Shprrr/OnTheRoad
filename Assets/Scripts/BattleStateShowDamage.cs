using UnityEngine;

public class BattleStateShowDamage : StateMachineBehaviour
{
    public BattleEvent battle;

    // Appelée sur la première image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        battle = (BattleEvent)animator.GetComponent<CurrentEvent>().currentEvent;

        animator.SetInteger("state", 0);
    }
}
