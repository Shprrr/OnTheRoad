using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    public CurrentEvent currentEvent;
    public GameObject profilePanelPrefab;

#if UNITY_EDITOR
    [IItemData(nameof(items))]
    [SerializeField]
#pragma warning disable IDE0044 // Ajouter un modificateur readonly
    private string itemsInspector = "";
#pragma warning restore IDE0044 // Ajouter un modificateur readonly
#else
    [SerializeField]
    private string itemsInspector = "";
#endif
    public IItemData[] items;
    public int money;
    public Text moneyText;

    // Cette fonction est appelée quand le script est chargé ou qu'une valeur est modifiée dans l'inspecteur (appelée dans l'éditeur uniquement)
    private void OnValidate()
    {
#if UNITY_EDITOR
        items = itemsInspector.FromXML<IItemData[]>();
#else
        items = new IItemData[] { new ItemUsableData { Id = "potionHp1", Amount = 4 }, new ItemUsableData { Id = "molotov", Amount = 1 }, new WeaponData { Id = "staff", Amount = 1 }, new EquipableData { Id = "shield", Amount = 2 }, new EquipableData { Id = "chestArmor", Amount = 1 }, new EquipableData { Id = "sleepBomb", Amount = 1 }, new EquipableData { Id = "confuseBomb", Amount = 1 } };
#endif
    }

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
#if !UNITY_EDITOR
        OnValidate();
#endif
        // Put items by Unity in the factory
        for (int i = 0; i < items.Length; i++)
        {
#if UNITY_EDITOR
            var foldout = items[i].foldout;
#endif
            items[i] = ItemFactory.Build(items[i].Id, items[i].Amount);
#if UNITY_EDITOR
            items[i].foldout = foldout;
#endif
        }
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        moneyText.text = money.ToString("### ##0").Trim();
    }

    public void AccessProfile(int battlerIndex)
    {
        var go = Instantiate(profilePanelPrefab, transform.parent);
        var profile = go.GetComponent<ProfileManager>();
        profile.battler = transform.GetChild(battlerIndex).GetComponent<Battler>();
        profile.currentEvent = currentEvent;
        //return go;
    }

    public void FullHeal()
    {
        foreach (var battler in GetComponentsInChildren<Battler>())
        {
            battler.Hp = battler.GetMaxHP();
            battler.Sp = battler.GetMaxSP();
        }
    }

    public void AddItem(IItemData item)
    {
        var index = Array.FindIndex(items, i => i.Name == item.Name);
        if (index >= 0)
            items[index].Amount += item.Amount;
        else
        {
            var list = items.ToList();
            list.Add(item);
            items = list.ToArray();
        }
    }

    public void DropItem(IItemData item, int amount = 1)
    {
        var index = Array.FindIndex(items, i => i.Name == item.Name);
        items[index].Amount -= amount;
        if (items[index].Amount < 1)
        {
            var list = items.ToList();
            list.RemoveAt(index);
            items = list.ToArray();
        }
    }
}
