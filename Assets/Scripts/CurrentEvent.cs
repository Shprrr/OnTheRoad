using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CTBManager), typeof(TargetSelectionManager))]
public class CurrentEvent : MonoBehaviour
{
    public GameObject nextMapPanelPrefab;
    public Map map;

    public MapPosition currentPosition;
    public MapEvent currentEvent;

    public Canvas canvas;

    public Party party;

    [Header("Directions")]
    public GameObject directions;
    public GameObject buttonLeft;
    public GameObject buttonMiddle;
    public GameObject buttonRight;
    public GameObject buttonBack;
    public GameObject itemsPanelPrefab;
    public GameObject skillsPanelPrefab;

    [HideInInspector]
    public TargetSelectionManager targetSelectionManager;
    [Header("Target Selection")]
    public GameObject targetSelection;
    public Button buttonBackTarget;

    [HideInInspector]
    public CTBManager ctbManager;
    [Header("Battle")]
    public GameObject battleCommands;
    public GameObject buttonAttack;
    public GameObject buttonLastCommand;
    public GameObject buttonSkills;
    public GameObject buttonItems;
    public GameObject buttonRun;
    public GameObject ctbPanel;

    public GameObject enemySpawn1;
    public GameObject enemySpawn2;
    public GameObject enemySpawn3;
    public GameObject chestPrefab;
    public GameObject resultPanelPrefab;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        ctbManager = GetComponent<CTBManager>();
        targetSelectionManager = GetComponent<TargetSelectionManager>();
        AccessNextMap();
    }

    public void Move(Direction direction)
    {
        var nextPosition = new MapPosition(currentPosition.X, currentPosition.Y);

        switch (direction)
        {
            case Direction.Back:
                nextPosition.Y -= 1;
                break;
            case Direction.Left:
                nextPosition.X -= 1;
                break;
            case Direction.Middle:
                nextPosition.Y += 1;
                break;
            case Direction.Right:
                nextPosition.X += 1;
                break;
        }

        Move(nextPosition);
    }

    public void Move(MapPosition position)
    {
        if (!map.mapData.ContainsKey(position))
            throw new KeyNotFoundException("Wrong direction.");

        map.mapData[position].Visited = true;
        currentPosition = position;
        currentEvent = map.mapData[position];

        RefreshPosition();
    }

    public void RefreshPosition()
    {
        targetSelection.SetActive(false);
        targetSelectionManager.onTargetSelected = null;
        targetSelectionManager.Enemies.Clear();

        battleCommands.SetActive(false);
        buttonAttack.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonLastCommand.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonSkills.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonItems.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonRun.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonBackTarget.onClick.RemoveAllListeners();
        ctbPanel.SetActive(false);
        GetComponent<Animator>().enabled = false;

        enemySpawn1.DestroyAllChildren();
        enemySpawn2.DestroyAllChildren();
        enemySpawn3.DestroyAllChildren();

        RefreshDirections();
        currentEvent.RefreshEvent(this);
    }

    public void RefreshDirections()
    {
        directions.SetActive(true);
        buttonLeft.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X - 1, currentPosition.Y)));
        buttonMiddle.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X, currentPosition.Y + 1)));
        buttonRight.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X + 1, currentPosition.Y)));
        buttonBack.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X, currentPosition.Y - 1)));
    }

    public GameObject AccessInventory(bool fullPanel = false)
    {
        var panel = Instantiate(itemsPanelPrefab, canvas.transform);
        if (fullPanel)
        {
            var rectTransform = panel.GetComponent<RectTransform>();
            rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 0.47f);
        }
        var manager = panel.GetComponent<ItemsManager>();
        manager.items = party.items;
        return panel;
    }

    public void AccessInventoryOutsideBattle()
    {
        var panel = AccessInventory(true);
        var manager = panel.GetComponent<ItemsManager>();
        manager.interactableItems = i => i is ItemUsableData && ((ItemUsableData)i).UsableOutsideBattle;
        manager.OnClick += ItemsManager_OnClick;
    }

    private void ItemsManager_OnClick(object sender, System.EventArgs e)
    {
        var item = (ItemUsableData)sender;
        var actor = targetSelectionManager.Actors[0];
        targetSelectionManager.ShowTargetChoice(actor, new BattleAction(BattleAction.BattleCommand.Items, item,
            new Cursor(Cursor.eTargetType.SINGLE_ENEMY, actor, item.TargetsPossible, targetSelectionManager.Actors, new List<Battler>())));
    }

    public GameObject AccessSkills()
    {
        return Instantiate(skillsPanelPrefab, canvas.transform);
    }

    public GameObject AccessNextMap()
    {
        var go = Instantiate(nextMapPanelPrefab, canvas.transform);
        var panel = go.GetComponent<NextMapManager>();
        panel.party = party;
        panel.templates = new MapTemplateData[] { new MapTemplateData("test"), new MapTemplateData("tutorial"), new MapTemplateData("greenhill") };
        panel.mapGenerator = map;
        return go;
    }
}
