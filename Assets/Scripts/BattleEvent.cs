using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BattleAction;
using UnityObject = UnityEngine.Object;

[Serializable]
public class BattleEvent : MapEvent
{
    public enum BattleResult
    {
        NONE,
        WIN,
        LOSE,
        ESCAPE
    }

    public int MAX_ACTOR = 2;
    public int MAX_ENEMY = 3;

    public CurrentEvent _currentEvent;

    private Animator animator;

    public GameObject prefabEnemy1;
    public GameObject prefabEnemy2;
    public GameObject prefabEnemy3;

    public bool finished;

    public List<Battler> Actors;
    public List<Battler> Enemies;
    public int ActiveBattlerIndex;
    public BattleResult Result;

    public BattleEvent(GameObject enemy1Prefab, GameObject enemy2Prefab = null, GameObject enemy3Prefab = null, string type = "Battle")
    {
        Type = type;
        prefabEnemy1 = enemy1Prefab;
        prefabEnemy2 = enemy2Prefab;
        prefabEnemy3 = enemy3Prefab;
        Actors = new List<Battler>(MAX_ACTOR);
        Enemies = new List<Battler>(MAX_ENEMY);
    }

    public override void RefreshEvent(CurrentEvent currentEvent)
    {
        base.RefreshEvent(currentEvent);

        _currentEvent = currentEvent;
        animator = currentEvent.GetComponent<Animator>();

        if (finished)
        {
            animator.enabled = false;
            return;
        }

        Actors.Clear();
        Actors.AddRange(currentEvent.party.GetComponentsInChildren<Battler>());

        Enemies.Clear();
        SpawnEnemy(prefabEnemy1, currentEvent.enemySpawn1.transform);
        SpawnEnemy(prefabEnemy2, currentEvent.enemySpawn2.transform);
        SpawnEnemy(prefabEnemy3, currentEvent.enemySpawn3.transform);

        currentEvent.targetSelectionManager.onTargetSelected = AfterAction;
        currentEvent.targetSelectionManager.Enemies.Clear();
        currentEvent.targetSelectionManager.Enemies.AddRange(Enemies);

        // Update target in last actions.
        foreach (var actor in Actors)
        {
            if (actor.lastAction.HasValue)
            {
                actor.lastAction.Value.Target.Actors.Clear();
                actor.lastAction.Value.Target.Actors.AddRange(Actors);
                actor.lastAction.Value.Target.Enemies.Clear();
                actor.lastAction.Value.Target.Enemies.AddRange(Enemies);
            }
        }

        currentEvent.directions.SetActive(false);
        currentEvent.battleCommands.SetActive(true);
        currentEvent.buttonAttack.GetComponent<Button>().onClick.AddListener(Attack);
        currentEvent.buttonLastCommand.GetComponent<Button>().onClick.AddListener(LastCommand);
        currentEvent.buttonSkills.GetComponent<Button>().onClick.AddListener(Skills);
        currentEvent.buttonItems.GetComponent<Button>().onClick.AddListener(Items);
        currentEvent.buttonRun.GetComponent<Button>().onClick.AddListener(Run);
        currentEvent.buttonBackTarget.onClick.AddListener(BackTarget);
        currentEvent.ctbPanel.SetActive(true);
        RefreshButtons(false);

        animator.enabled = true;
        animator.Rebind();
        _currentEvent.ctbManager.StartBattle();
    }

    private void SpawnEnemy(GameObject prefabEnemy, Transform transform)
    {
        if (prefabEnemy != null)
        {
            var enemyGO = UnityObject.Instantiate(prefabEnemy, transform);
            var enemy = enemyGO.GetComponent<Battler>();
            Enemies.Add(enemy);
        }
    }

    public void RefreshButtons(bool activateButtons)
    {
        if (!GetActiveBattler().IsPlayer) return;

        if (GetActiveBattler().BattlerStatus.GetRestriction().HasValue) activateButtons = false;

        //Array.ForEach(_currentEvent.battleCommands.GetComponentsInChildren<Button>(), b => b.interactable = activateButtons);
        _currentEvent.buttonAttack.GetComponent<Button>().interactable = activateButtons && GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandAttackId) == 1;
        _currentEvent.buttonSkills.GetComponent<Button>().interactable = activateButtons && GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandSkillsId) == 1;
        _currentEvent.buttonItems.GetComponent<Button>().interactable = activateButtons && GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandItemsId) == 1;
        _currentEvent.buttonRun.GetComponent<Button>().interactable = activateButtons && GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandRunId) == 1;
        var buttonLastCommand = _currentEvent.buttonLastCommand.GetComponent<Button>();

        var lastAction = GetActiveBattler().lastAction;
        buttonLastCommand.interactable = lastAction.HasValue && activateButtons;

        if (!lastAction.HasValue)
            buttonLastCommand.GetComponentInChildren<Text>().text = "Last Command";
        else switch (lastAction.Value.Kind)
            {
                case BattleCommand.Nothing:
                    buttonLastCommand.GetComponentInChildren<Text>().text = "Do Nothing";
                    break;
                case BattleCommand.Attack:
                    buttonLastCommand.interactable = GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandAttackId) == 1;
                    buttonLastCommand.GetComponentInChildren<Text>().text = "Attack";
                    break;
                case BattleCommand.Skills:
                    var lastSkill = (SkillData)lastAction.Value.Data;
                    buttonLastCommand.interactable = GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandSkillsId) == 1;
                    if (lastSkill.SpCost > GetActiveBattler().Sp)
                        buttonLastCommand.interactable = false;
                    buttonLastCommand.GetComponentInChildren<Text>().text = lastSkill.Name + "\nSP Cost:" + lastSkill.SpCost;
                    break;
                case BattleCommand.Items:
                    var lastItem = (ItemUsableData)lastAction.Value.Data;
                    buttonLastCommand.interactable = GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandItemsId) == 1;
                    if (lastItem.Amount == 0)
                        buttonLastCommand.interactable = false;
                    buttonLastCommand.GetComponentInChildren<Text>().text = lastItem.Name + "\nAmount:" + lastItem.Amount;
                    break;
                case BattleCommand.Run:
                    buttonLastCommand.interactable = GetActiveBattler().GetCalculatedStat(CharacteristicFactory.BattleCommandRunId) == 1;
                    buttonLastCommand.GetComponentInChildren<Text>().text = "Run";
                    break;
            }
    }

    /// <summary>
    /// Get the Battler who is taking an action.
    /// </summary>
    /// <returns></returns>
    public Battler GetActiveBattler()
    {
        return ActiveBattlerIndex < MAX_ACTOR ? Actors[ActiveBattlerIndex] : Enemies[ActiveBattlerIndex - MAX_ACTOR];
    }

    /// <summary>
    /// Determine battle Win/Loss results.
    /// </summary>
    /// <returns></returns>
    public bool Judge()
    {
        int nbBattlerAlive = Actors.Count(b => !b.CantFight);
        if (nbBattlerAlive == 0)
        {
            //Result = CanLose ? eBattleResult.LOSE : eBattleResult.ESCAPE;
            Result = BattleResult.LOSE;
            return true;
        }

        nbBattlerAlive = Enemies.Count(b => !b.CantFight);
        if (nbBattlerAlive == 0)
        {
            Result = BattleResult.WIN;
            return true;
        }

        return false;
    }

    public void FinishBattle()
    {
        finished = true;
        _currentEvent.RefreshPosition();
    }

    private void AfterAction()
    {
        //TODO: LastAction ne modifie que dans une battle ou non ?
        //getActiveBattler().lastAction = action;
        RefreshButtons(false);
        animator.SetInteger("state", 2);

        if (GetActiveBattler().lastAction.Value.Kind == BattleCommand.Nothing)
            _currentEvent.StartCoroutine(AfterActionNothing());

        IEnumerator AfterActionNothing()
        {
            yield return new WaitForSeconds(1);
            animator.SetInteger("state", 0);
        }
    }

    public void Attack()
    {
        var action = new BattleAction(BattleCommand.Attack, target: new Cursor(Cursor.eTargetType.SINGLE_ENEMY, GetActiveBattler(), Cursor.POSSIBLE_TARGETS_ONE, Actors, Enemies));
        _currentEvent.targetSelectionManager.ShowTargetChoice(GetActiveBattler(), action);
        lastCommand = BattleCommand.Attack;
    }

    private BattleCommand lastCommand;
    public void BackTarget()
    {
        RefreshButtons(true);
        switch (lastCommand)
        {
            case BattleCommand.Skills:
                Skills();
                break;
            case BattleCommand.Items:
                Items();
                break;
        }
    }

    public void LastCommand()
    {
        var lastAction = GetActiveBattler().lastAction;
        //Debug.LogFormat("LastCommand => {0} !", lastAction.Value);
        switch (lastAction.Value.Kind)
        {
            default:
            case BattleCommand.Nothing:
                break;
            case BattleCommand.Attack:
                Attack();
                break;
            case BattleCommand.Skills:
            case BattleCommand.Items:
                _currentEvent.targetSelectionManager.ShowTargetChoice(GetActiveBattler(), lastAction.Value);
                break;
            case BattleCommand.Run:
                Run();
                break;
        }
    }

    public void Skills()
    {
        var skillManager = _currentEvent.AccessSkills().GetComponent<SkillsManager>();
        skillManager.user = GetActiveBattler();
        skillManager.skills = GetActiveBattler().Skills;
        skillManager.OnClick += (sender, e) =>
            {
                var skill = (SkillData)sender;
                var action = new BattleAction(BattleCommand.Skills, skill,
                    new Cursor(Cursor.eTargetType.SINGLE_ENEMY, GetActiveBattler(), skill.TargetsPossible, Actors, Enemies));
                _currentEvent.targetSelectionManager.ShowTargetChoice(GetActiveBattler(), action);
            };

        lastCommand = BattleCommand.Skills;
    }

    public void Items()
    {
        var manager = _currentEvent.AccessInventory().GetComponent<ItemsManager>();
        manager.interactableItems = i => i is ItemUsableData;
        manager.OnClick += (sender, e) =>
          {
              var item = (ItemUsableData)sender;
              var action = new BattleAction(BattleCommand.Items, item,
                  new Cursor(Cursor.eTargetType.SINGLE_ENEMY, GetActiveBattler(), item.TargetsPossible, Actors, Enemies));
              _currentEvent.targetSelectionManager.ShowTargetChoice(GetActiveBattler(), action);
          };

        lastCommand = BattleCommand.Items;
    }

    public void Run()
    {
        lastCommand = BattleCommand.Run;
        RefreshButtons(false);
        _currentEvent.Move(Direction.Back);
    }
}
