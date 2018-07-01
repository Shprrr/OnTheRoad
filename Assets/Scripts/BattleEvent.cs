using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityObject = UnityEngine.Object;

public class BattleEvent : MapEvent
{
    public enum BattleCommand
    {
        Nothing,
        Attack,
        Skills,
        Items,
        Run
    }

    public int MAX_ACTOR = 2;
    public int MAX_ENEMY = 3;

    private CurrentEvent _currentEvent;
    private CTBManager ctbManager;

    public GameObject prefabEnemy1;
    public GameObject prefabEnemy2;
    public GameObject prefabEnemy3;

    public List<Battler> Actors;
    public List<Battler> Enemies;
    public int ActiveBattlerIndex;

    public BattleEvent()
    {
        Actors = new List<Battler>(MAX_ACTOR);
        Enemies = new List<Battler>(MAX_ENEMY);
    }

    public override void RefreshEvent(CurrentEvent currentEvent)
    {
        base.RefreshEvent(currentEvent);

        _currentEvent = currentEvent;
        ctbManager = _currentEvent.ctbManager;

        Actors.Clear();
        Actors.AddRange(currentEvent.party.GetComponentsInChildren<Battler>());

        Enemies.Clear();
        SpawnEnemy(prefabEnemy1, currentEvent.enemySpawn1.transform);
        SpawnEnemy(prefabEnemy2, currentEvent.enemySpawn2.transform);
        SpawnEnemy(prefabEnemy3, currentEvent.enemySpawn3.transform);

        currentEvent.directions.SetActive(false);
        currentEvent.battleCommands.SetActive(true);
        currentEvent.buttonAttack.GetComponent<Button>().onClick.AddListener(Attack);
        currentEvent.buttonLastCommand.GetComponent<Button>().onClick.AddListener(LastCommand);
        currentEvent.buttonSkills.GetComponent<Button>().onClick.AddListener(Skills);
        currentEvent.buttonItems.GetComponent<Button>().onClick.AddListener(Items);
        currentEvent.buttonRun.GetComponent<Button>().onClick.AddListener(Run);
        RefreshButtons();

        StartBattle();
    }

    private void SpawnEnemy(GameObject prefabEnemy, Transform transform)
    {
        if (prefabEnemy != null)
        {
            var enemy = UnityObject.Instantiate(prefabEnemy, transform);
            Enemies.Add(enemy.GetComponent<Battler>());
        }
    }

    private void RefreshButtons()
    {
        var buttonLastCommand = _currentEvent.buttonLastCommand.GetComponent<Button>();
        buttonLastCommand.interactable = lastCommand != BattleCommand.Nothing;

        switch (lastCommand)
        {
            default:
            case BattleCommand.Nothing:
                buttonLastCommand.GetComponentInChildren<Text>().text = "Last Command";
                break;
            case BattleCommand.Attack:
                buttonLastCommand.GetComponentInChildren<Text>().text = "Attack";
                break;
            case BattleCommand.Skills:
                buttonLastCommand.GetComponentInChildren<Text>().text = "Skills";
                break;
            case BattleCommand.Items:
                buttonLastCommand.GetComponentInChildren<Text>().text = "Items";
                break;
            case BattleCommand.Run:
                buttonLastCommand.GetComponentInChildren<Text>().text = "Run";
                break;
        }
    }

    public void StartBattle()
    {
        ctbManager.CalculateCTB(this);
        //BeginTurn();
    }

    public void Attack()
    {
        Debug.Log("Attack !");
        lastCommand = BattleCommand.Attack;
        RefreshButtons();
    }

    private BattleCommand lastCommand;
    public void LastCommand()
    {
        Debug.LogFormat("LastCommand => {0} !", lastCommand);
        switch (lastCommand)
        {
            default:
            case BattleCommand.Nothing:
                break;
            case BattleCommand.Attack:
                Attack();
                break;
            case BattleCommand.Skills:
                Skills();
                break;
            case BattleCommand.Items:
                Items();
                break;
            case BattleCommand.Run:
                Run();
                break;
        }
    }

    public void Skills()
    {
        Debug.Log("Skills !");
        lastCommand = BattleCommand.Skills;
        RefreshButtons();
    }

    public void Items()
    {
        Debug.Log("Items !");
        lastCommand = BattleCommand.Items;
        RefreshButtons();
    }

    public void Run()
    {
        lastCommand = BattleCommand.Run;
        RefreshButtons();
        _currentEvent.Move(Direction.Back);
    }
}
