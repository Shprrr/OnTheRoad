using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    public ItemData[] items;
    public int money;
    public Text moneyText;

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

    public void FullHeal()
    {
        foreach (var battler in GetComponentsInChildren<Battler>())
        {
            battler.Hp = battler.MaxHP;
            battler.Sp = battler.MaxSP;
        }
    }

    public void AddItem(ItemData item)
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

    public void DropItem(ItemData item, int amount = 1)
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
