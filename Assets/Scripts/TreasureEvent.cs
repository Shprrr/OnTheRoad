using System;

[Serializable]
public class TreasureEvent : MapEvent
{
    private CurrentEvent _currentEvent;

    public IItemData[] ItemsInChest;
    public int MoneyInChest;
    public bool Opened;

    public TreasureEvent(IItemData[] itemsInChest, int moneyInChest)
    {
        Type = "Treasure";
        ItemsInChest = itemsInChest;
        MoneyInChest = moneyInChest;
    }

    public override void RefreshEvent(CurrentEvent currentEvent)
    {
        base.RefreshEvent(currentEvent);

        _currentEvent = currentEvent;

        var go = UnityEngine.Object.Instantiate(currentEvent.chestPrefab, currentEvent.enemySpawn1.transform);
        var chest = go.GetComponent<Chest>();
        chest.currentEvent = this;
        chest.open = Opened;
    }

    public void Open()
    {
        if (MoneyInChest == 0 && ItemsInChest.Length == 0)
            return;

        var result = UnityEngine.Object.Instantiate(_currentEvent.resultPanelPrefab, _currentEvent.canvas.transform).GetComponent<BattleResult>();
        result.currentEvent = _currentEvent;
        result.gameOver = false;
        result.moneyGained = MoneyInChest;
        result.itemsGained = ItemsInChest;
        result.titleText.gameObject.SetActive(false);

        // Empty the chest.
        MoneyInChest = 0;
        ItemsInChest = Array.Empty<IItemData>();
    }
}
