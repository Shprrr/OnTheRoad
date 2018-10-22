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
    public string itemsInspector;
#endif
    public IItemData[] items;
    public int money;
    public Text moneyText;

    // Cette fonction est appelée quand le script est chargé ou qu'une valeur est modifiée dans l'inspecteur (appelée dans l'éditeur uniquement)
    private void OnValidate()
    {
        items = itemsInspector.FromXML<IItemData[]>();
    }

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        // Put items by Unity in the factory
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = ItemFactory.Build(items[i].Id, items[i].Amount);
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
            battler.Hp = battler.MaxHP;
            battler.Sp = battler.MaxSP;
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
