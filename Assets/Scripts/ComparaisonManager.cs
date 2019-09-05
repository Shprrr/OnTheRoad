using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(BattlerStatsUI), typeof(BattlerStatsUI))]
public class ComparaisonManager : MonoBehaviour
{
    private Animator animator;
    private BattlerStatsUI oldStatsUI;
    private BattlerStatsUI newStatsUI;

    public Color positiveColor;
    public Color negativeColor;

    public CurrentEvent currentEvent;
    public Battler battler;
    public EquipableData equipableData;
    public EquipmentSlot slot;

    public Image portrait;
    public Text itemFrom;
    public Text itemTo;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        animator = GetComponent<Animator>();
        oldStatsUI = GetComponent<BattlerStatsUI>();
        newStatsUI = GetComponents<BattlerStatsUI>()[1];
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        oldStatsUI.battler = battler;
        newStatsUI.battler = Instantiate(battler);
        ChangeItem(newStatsUI.battler);

        portrait.GetComponent<Image>().sprite = battler.GetComponentInChildren<Image>().sprite;
        itemFrom.text = GetItemFromSlot(battler)?.Name ?? "- NONE -";
        itemTo.text = equipableData?.Name ?? "- NONE -";

        ChangeColor(newStatsUI.maxHP, oldStatsUI.battler.GetMaxHP(), newStatsUI.battler.GetMaxHP());
        ChangeColor(newStatsUI.maxSP, oldStatsUI.battler.GetMaxSP(), newStatsUI.battler.GetMaxSP());
        ChangeColor(newStatsUI.strength, oldStatsUI.battler.GetStrength(), newStatsUI.battler.GetStrength());
        ChangeColor(newStatsUI.vitality, oldStatsUI.battler.GetVitality(), newStatsUI.battler.GetVitality());
        ChangeColor(newStatsUI.intellect, oldStatsUI.battler.GetIntellect(), newStatsUI.battler.GetIntellect());
        ChangeColor(newStatsUI.wisdom, oldStatsUI.battler.GetWisdom(), newStatsUI.battler.GetWisdom());
        ChangeColor(newStatsUI.agility, oldStatsUI.battler.GetAgility(), newStatsUI.battler.GetAgility());

        ChangeColor(newStatsUI.physicalDamage, (oldStatsUI.battler.getMinBaseDamage() + oldStatsUI.battler.getMaxBaseDamage()) / 2, (newStatsUI.battler.getMinBaseDamage() + newStatsUI.battler.getMaxBaseDamage()) / 2);
        ChangeColor(newStatsUI.physicalAccuracy, oldStatsUI.battler.getAttackMultiplier() * oldStatsUI.battler.getHitPourc(), newStatsUI.battler.getAttackMultiplier() * newStatsUI.battler.getHitPourc());
        ChangeColor(newStatsUI.physicalDefense, oldStatsUI.battler.getDefenseDamage(), newStatsUI.battler.getDefenseDamage());
        ChangeColor(newStatsUI.physicalEvasion, oldStatsUI.battler.getDefenseMultiplier() * oldStatsUI.battler.getEvadePourc(), newStatsUI.battler.getDefenseMultiplier() * newStatsUI.battler.getEvadePourc());

        ChangeColor(newStatsUI.magicalDamage, (oldStatsUI.battler.getMagicMinBaseDamage(0) + oldStatsUI.battler.getMagicMaxBaseDamage(0)) / 2, (newStatsUI.battler.getMagicMinBaseDamage(0) + newStatsUI.battler.getMagicMaxBaseDamage(0)) / 2);
        ChangeColor(newStatsUI.magicalAccuracy, oldStatsUI.battler.getMagicAttackMultiplier() * oldStatsUI.battler.getMagicHitPourc(100), newStatsUI.battler.getMagicAttackMultiplier() * newStatsUI.battler.getMagicHitPourc(100));
        ChangeColor(newStatsUI.magicalDefense, oldStatsUI.battler.getMagicDefenseDamage(), newStatsUI.battler.getMagicDefenseDamage());
        ChangeColor(newStatsUI.magicalEvasion, oldStatsUI.battler.getMagicDefenseMultiplier() * oldStatsUI.battler.getMagicEvadePourc(), newStatsUI.battler.getMagicDefenseMultiplier() * newStatsUI.battler.getMagicEvadePourc());
    }

    private void ChangeColor(Text textToChange, int oldValue, int newValue)
    {
        if (newValue > oldValue)
            textToChange.color = positiveColor;
        if (newValue < oldValue)
            textToChange.color = negativeColor;
    }

    public void Confirm()
    {
        EquipableData oldItem = GetItemFromSlot(battler);

        ChangeItem(battler);

        if (oldItem != null)
        {
            oldItem.Amount = 1;
            currentEvent.party.AddItem(oldItem);
        }
        if (equipableData != null)
            currentEvent.party.DropItem(equipableData);

        Back();
    }

    private EquipableData GetItemFromSlot(Battler battler)
    {
        EquipableData oldItem = null;
        switch (slot)
        {
            case EquipmentSlot.Weapon:
                oldItem = battler.Weapon;
                break;
            case EquipmentSlot.Offhand:
                oldItem = battler.Offhand;
                break;
            case EquipmentSlot.Head:
                oldItem = battler.Head;
                break;
            case EquipmentSlot.Body:
                oldItem = battler.Body;
                break;
            case EquipmentSlot.Feet:
                oldItem = battler.Feet;
                break;
            case EquipmentSlot.Neck:
                oldItem = battler.Neck;
                break;
            case EquipmentSlot.Finger1:
                oldItem = battler.Finger1;
                break;
            case EquipmentSlot.Finger2:
                oldItem = battler.Finger2;
                break;
        }

        return oldItem;
    }

    private void ChangeItem(Battler battler)
    {
        switch (slot)
        {
            case EquipmentSlot.Weapon:
                battler.Weapon = (WeaponData)equipableData;
                break;
            case EquipmentSlot.Offhand:
                battler.Offhand = equipableData;
                break;
            case EquipmentSlot.Head:
                battler.Head = equipableData;
                break;
            case EquipmentSlot.Body:
                battler.Body = equipableData;
                break;
            case EquipmentSlot.Feet:
                battler.Feet = equipableData;
                break;
            case EquipmentSlot.Neck:
                battler.Neck = equipableData;
                break;
            case EquipmentSlot.Finger1:
                battler.Finger1 = equipableData;
                break;
            case EquipmentSlot.Finger2:
                battler.Finger2 = equipableData;
                break;
        }
    }

    public void Back()
    {
        Destroy(newStatsUI.battler.gameObject);
        animator.SetTrigger("close");
    }
}
