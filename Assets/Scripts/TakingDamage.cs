using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TakingDamage : MonoBehaviour
{
    private Animator animator;

    public AnimatorOverrideController animationAttack;
    public Damage damage;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        animator.runtimeAnimatorController = animationAttack;

        GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
    }

    // Cette fonction est appelée quand le MonoBehaviour est détruit
    private void OnDestroy()
    {
        if (damage != Damage.Empty)
            damage.ApplyDamage();
    }
}
