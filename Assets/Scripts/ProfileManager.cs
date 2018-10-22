using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(BattlerStatsUI))]
public class ProfileManager : MonoBehaviour
{
    private Animator animator;
    private BattlerStatsUI statsUI;

    public GameObject hpSliderPrefab;
    public GameObject spSliderPrefab;

    public CurrentEvent currentEvent;
    public Battler battler;
    public Image portrait;
    public Transform hpSliderPosition;
    public Transform spSliderPosition;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        animator = GetComponent<Animator>();
        statsUI = GetComponent<BattlerStatsUI>();
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        statsUI.battler = battler;
        portrait.GetComponent<Image>().sprite = battler.GetComponentInChildren<Image>().sprite;
        var go = Instantiate(hpSliderPrefab, hpSliderPosition);
        go.GetComponent<HPBar>().battler = battler;
        go = Instantiate(spSliderPrefab, spSliderPosition);
        go.GetComponent<SPBar>().battler = battler;
    }

    public void Back()
    {
        animator.SetTrigger("close");
    }

    public void ChangeWeapon()
    {
        // Pop une liste des weapons possibles.
        var panel = currentEvent.AccessInventory();

        // Quand on choisit un item, ça l'équipe dans la slot de weapon.
    }

    public void ChangeOffhand()
    {
    }
}
