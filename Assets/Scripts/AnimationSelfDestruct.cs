using UnityEngine;

public class AnimationSelfDestruct : StateMachineBehaviour
{
    // Appelée sur la dernière image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
