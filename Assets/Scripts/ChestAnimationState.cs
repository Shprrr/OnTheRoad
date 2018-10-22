using UnityEngine;

public class ChestAnimationState : StateMachineBehaviour
{
    // Appelée sur la dernière image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animator.GetBool("open"))
            animator.GetComponent<Chest>().currentEvent.Open();
    }
}
