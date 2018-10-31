using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(BattlerStatsUI))]
public class ProfileManager : MonoBehaviour
{
    private Animator animator;
    private BattlerStatsUI statsUI;

    public GameObject hpSliderPrefab;
    public GameObject spSliderPrefab;
    public GameObject comparaisonPrefab;

    public CurrentEvent currentEvent;
    public Battler battler;
    public Image portrait;
    public Transform hpSliderPosition;
    public Transform spSliderPosition;

    public Transform statsGroup;
    public Transform equipmentGroup;
    public Button equipButton;

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

        if (currentEvent.currentEvent is BattleEvent && !((BattleEvent)currentEvent.currentEvent).finished)
        {
            foreach (var button in equipmentGroup.GetComponentsInChildren<Button>())
            {
                button.interactable = false;
            }
        }
    }

    public void Back()
    {
        animator.SetTrigger("close");
    }

    public void Equip()
    {
        equipmentGroup.gameObject.SetActive(!equipmentGroup.gameObject.activeSelf);
        statsGroup.gameObject.SetActive(!equipmentGroup.gameObject.activeSelf);
        equipButton.GetComponentInChildren<Text>().text = equipmentGroup.gameObject.activeSelf ? "Stats" : "Equip";
    }

    private EquipmentSlot changingSlot;
    public void ChangeEquipment(ProfileEquipmentSlot slot)
    {
        // Pop une liste des weapons possibles.
        var panel = currentEvent.AccessInventory(true);
        var manager = panel.GetComponent<ItemsManager>();

        changingSlot = slot.equipmentSlot;
        EquipableData.EquipmentSlot equipSlot = EquipableData.EquipmentSlot.Weapon;
        switch (slot.equipmentSlot)
        {
            case EquipmentSlot.Weapon:
                equipSlot = EquipableData.EquipmentSlot.Weapon;
                break;
            case EquipmentSlot.Offhand:
                equipSlot = EquipableData.EquipmentSlot.Offhand;
                break;
            case EquipmentSlot.Head:
                equipSlot = EquipableData.EquipmentSlot.Head;
                break;
            case EquipmentSlot.Body:
                equipSlot = EquipableData.EquipmentSlot.Body;
                break;
            case EquipmentSlot.Feet:
                equipSlot = EquipableData.EquipmentSlot.Feet;
                break;
            case EquipmentSlot.Neck:
                equipSlot = EquipableData.EquipmentSlot.Neck;
                break;
            case EquipmentSlot.Finger1:
            case EquipmentSlot.Finger2:
                equipSlot = EquipableData.EquipmentSlot.Finger;
                break;
        }
        manager.interactableItems = i => i is EquipableData && ((EquipableData)i).Slot == equipSlot;
        // Ajout de l'option "Upequip"
        manager.items = manager.items.Prepend(new EquipableData { Name = "- NONE -", Slot = equipSlot }).ToArray();

        // Quand on choisit un item, ça montre un différentiel et confirm pour équiper dans la slot.
        manager.OnClick += Manager_OnClick;
    }

    private void Manager_OnClick(object sender, System.EventArgs e)
    {
        var item = (EquipableData)sender;
        if (string.IsNullOrEmpty(item.Id))
            item = null;

        var panel = Instantiate(comparaisonPrefab, transform.parent);
        var comparaison = panel.GetComponent<ComparaisonManager>();
        comparaison.currentEvent = currentEvent;
        comparaison.battler = battler;
        comparaison.equipableData = item;
        comparaison.slot = changingSlot;
    }
}
