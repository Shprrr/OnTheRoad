using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TakingDamage : MonoBehaviour
{
    public Color damageHPColor;
    public Color damageMPColor;
    public Color healHPColor;
    public Color healMPColor;

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

        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = damage.Multiplier == 0 ? "MISS" : System.Math.Abs(damage.Value).ToString();
        if (damage.Value < 0)
            if (damage.Type == Damage.eDamageType.MP)
                text.color = healMPColor;
            else text.color = healHPColor;
        else if (damage.Type == Damage.eDamageType.MP)
            text.color = damageMPColor;
        else
            text.color = damageHPColor;
    }

    // Cette fonction est appelée quand le MonoBehaviour est détruit
    private void OnDestroy()
    {
        if (damage != Damage.Empty)
            damage.ApplyDamage();

        GameObject.FindGameObjectWithTag("GameController").GetComponent<Animator>().SetInteger("state", 0);
    }
}
