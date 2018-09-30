using System;

[Serializable]
public class TreasureEvent : MapEvent
{
    public ItemData[] ItemsInChest;
    public int MoneyInChest;
    public bool Open;

    public TreasureEvent(ItemData[] itemsInChest, int moneyInChest)
    {
        Type = "Treasure";
        ItemsInChest = itemsInChest;
        MoneyInChest = moneyInChest;
    }

    public override void RefreshEvent(CurrentEvent currentEvent)
    {
        base.RefreshEvent(currentEvent);

        var go = UnityEngine.Object.Instantiate(currentEvent.chestPrefab, currentEvent.enemySpawn1.transform);
        var chest = go.GetComponent<Chest>();
        chest.currentEvent = this;
        chest.open = Open;
    }
}
