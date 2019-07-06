using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class TakingDamage : MonoBehaviour
{
    public GameObject attackNamePanelPrefab;
    public Color damageHPColor;
    public Color damageMPColor;
    public Color healHPColor;
    public Color healMPColor;

    private Animator animator;
    private GameObject attackNamePanel;

    public bool nextTurn;
    public bool showAnimation;
    public AnimatorOverrideController animationAttack;
    public bool showDamage;
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

        if (showAnimation)
        {
            attackNamePanel = Instantiate(attackNamePanelPrefab, GameObject.FindGameObjectWithTag("GameController").GetComponent<CurrentEvent>().canvas.transform);
            attackNamePanel.GetComponentInChildren<Text>().text = damage.Name;
        }

        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = damage.Multiplier == 0 ? "MISS" : System.Math.Abs(damage.Value).ToString();
        if (damage.Value < 0)
            if (damage.Type == Damage.DamageType.MP)
                text.color = healMPColor;
            else text.color = healHPColor;
        else if (damage.Type == Damage.DamageType.MP)
            text.color = damageMPColor;
        else
            text.color = damageHPColor;
    }

    // Cette fonction est appelée quand le MonoBehaviour est détruit
    private void OnDestroy()
    {
        if (showAnimation)
            Destroy(attackNamePanel);

        if (damage != Damage.Empty)
            damage.ApplyDamage();

        if (nextTurn)
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Animator>().SetInteger("state", 0);
    }

    public void AdjustAnimation()
    {
        GetComponent<Image>().enabled = showAnimation;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false;
    }

    public void AdjustDamage()
    {
        GetComponent<Image>().enabled = false;
        GetComponentInChildren<TextMeshProUGUI>().enabled = showDamage;
    }
}
