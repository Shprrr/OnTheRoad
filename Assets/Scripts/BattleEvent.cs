using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BattleAction;
using UnityObject = UnityEngine.Object;

[Serializable]
public class BattleEvent : MapEvent
{
    public enum eBattleResult
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
    public eBattleResult Result;

    public BattleEvent()
    {
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

        currentEvent.directions.SetActive(false);
        currentEvent.battleCommands.SetActive(true);
        currentEvent.buttonAttack.GetComponent<Button>().onClick.AddListener(Attack);
        currentEvent.buttonLastCommand.GetComponent<Button>().onClick.AddListener(LastCommand);
        currentEvent.buttonSkills.GetComponent<Button>().onClick.AddListener(Skills);
        currentEvent.buttonItems.GetComponent<Button>().onClick.AddListener(Items);
        currentEvent.buttonRun.GetComponent<Button>().onClick.AddListener(Run);
        currentEvent.buttonBackTarget.GetComponent<Button>().onClick.AddListener(BackTarget);
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
        if (!getActiveBattler().IsPlayer) return;
        Array.ForEach(_currentEvent.battleCommands.GetComponentsInChildren<Button>(true), b => b.gameObject.SetActive(true));
        _currentEvent.buttonBackTarget.SetActive(false);
        Array.ForEach(_currentEvent.battleCommands.GetComponentsInChildren<Button>(), b => b.interactable = activateButtons);
        _currentEvent.buttonItems.GetComponent<Button>().interactable = false;//TODO: Items not implemented.
        var buttonLastCommand = _currentEvent.buttonLastCommand.GetComponent<Button>();
        var lastAction = getActiveBattler().lastAction;
        buttonLastCommand.interactable = lastAction.HasValue && activateButtons;

        if (!lastAction.HasValue)
            buttonLastCommand.GetComponentInChildren<Text>().text = "Last Command";
        else switch (lastAction.Value.Kind)
            {
                case BattleCommand.Nothing:
                    buttonLastCommand.GetComponentInChildren<Text>().text = "Do Nothing";
                    break;
                case BattleCommand.Attack:
                    buttonLastCommand.GetComponentInChildren<Text>().text = "Attack";
                    break;
                case BattleCommand.Skills:
                    buttonLastCommand.GetComponentInChildren<Text>().text = lastAction.Value.Skill.Name + "  SP Cost:" + lastAction.Value.Skill.SpCost;
                    break;
                case BattleCommand.Items:
                    //TODO: Replace text by last item used and amount remaining.
                    buttonLastCommand.GetComponentInChildren<Text>().text = "Items";
                    break;
                case BattleCommand.Run:
                    buttonLastCommand.GetComponentInChildren<Text>().text = "Run";
                    break;
            }
    }

    private List<GameObject> targetsPossible = new List<GameObject>();
    public void ShowTargetChoice(BattleAction action)
    {
        Array.ForEach(_currentEvent.battleCommands.GetComponentsInChildren<Button>(), b => b.gameObject.SetActive(false));
        _currentEvent.buttonBackTarget.SetActive(true);

        bool multi = false, all = false;
        List<Battler> targets = new List<Battler>();
        foreach (var targetPossible in action.Target.PossibleTargets)
        {
            switch (targetPossible)
            {
                case Cursor.eTargetType.MULTI_ENEMY:
                    multi = true;
                    goto case Cursor.eTargetType.SINGLE_ENEMY;
                case Cursor.eTargetType.SINGLE_ENEMY:
                    targets.AddRange(Enemies);
                    break;
                case Cursor.eTargetType.MULTI_PARTY:
                    multi = true;
                    goto case Cursor.eTargetType.SINGLE_PARTY;
                case Cursor.eTargetType.SINGLE_PARTY:
                    targets.AddRange(Actors);
                    break;
                case Cursor.eTargetType.SELF:
                    targets.Add(action.Target.Self);
                    break;
                case Cursor.eTargetType.ALL:
                    all = multi = true;
                    targets.AddRange(Actors);
                    targets.AddRange(Enemies);
                    break;
            }
        }
        // Remove self target if the party is also targeted.
        targets = targets.Distinct().ToList();

        foreach (var targetPossible in targets)
        {
            var targetGO = UnityObject.Instantiate(targetPossible.targetPrefab, targetPossible.transform);
            //if (multi)
            //{
            //    var images = targetGO.GetComponentsInChildren<Image>();
            //    var color = images[1].color;
            //    color.a = 0.5f;
            //    images[1].color = color;
            //}
            var animator = targetGO.GetComponentInChildren<Animator>();
            animator.SetBool("multi", multi);
            animator.SetBool("all", all);
            animator.enabled = true;
            targetsPossible.Add(targetGO);

            var target = targetGO.GetComponent<TargetSelection>();
            target.target = targetPossible;
            if (all)
                target.targetType = Cursor.eTargetType.ALL;
            else if (multi)
                if (Actors.Contains(targetPossible))
                    target.targetType = Cursor.eTargetType.MULTI_PARTY;
                else
                    target.targetType = Cursor.eTargetType.MULTI_ENEMY;
            else
                if (Actors.Contains(targetPossible))
                target.targetType = Cursor.eTargetType.SINGLE_PARTY;
            else
                target.targetType = Cursor.eTargetType.SINGLE_ENEMY;

            target.action = action;
            target.battle = this;
        }
    }

    public void RemoveTargetChoice()
    {
        foreach (var target in targetsPossible)
        {
            UnityObject.Destroy(target);
        }
    }

    /// <summary>
    /// Get the Battler who is taking an action.
    /// </summary>
    /// <returns></returns>
    public Battler getActiveBattler()
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
            Result = eBattleResult.LOSE;
            return true;
        }

        nbBattlerAlive = Enemies.Count(b => !b.CantFight);
        if (nbBattlerAlive == 0)
        {
            Result = eBattleResult.WIN;
            return true;
        }

        return false;
    }

    public void FinishBattle()
    {
        finished = true;
        _currentEvent.RefreshPosition();
    }

    public void ActionState(BattleAction action)
    {
        var targets = action.Target.getTargetBattler();
        switch (action.Kind)
        {
            case BattleCommand.Attack:
                for (int i = 0; i < targets.Count; i++)
                    if (targets[i] != null)
                        getActiveBattler().Attacks(targets[i]);
                break;

            case BattleCommand.Skills:
                {
                    int skillLevel;
                    if (!getActiveBattler().Casts(action.Skill, out skillLevel))
                        break;

                    int nbTarget = 0;
                    for (int i = 0; i < targets.Count; i++)
                        if (targets[i] != null)
                            nbTarget++;

                    for (int i = 0; i < targets.Count; i++)
                        if (targets[i] != null)
                            getActiveBattler().Used(targets[i], action.Skill, skillLevel, nbTarget);
                }
                break;

            //case BattleCommand.Items
            //    {
            //        int nbTarget = 0;
            //        for (int i = 0; i < targets.Count; i++)
            //            if (targets[i] != null)
            //                nbTarget++;

            //        for (int i = 0; i < targets.Count; i++)
            //            if (targets[i] != null)
            //                targets[i].Used(getActiveBattler(), action.Item, nbTarget);

            //        if (getActiveBattler().IsPlayer)
            //        {
            //            Player.GamePlayer.Inventory.Drop(action.Item);
            //            _ItemSelection.RefreshChoices();
            //        }
            //    }
            //    break;

            case BattleCommand.Run:
                break;

            case BattleCommand.Nothing:
                break;
        }
        getActiveBattler().lastAction = action;
        RefreshButtons(false);
        animator.SetInteger("state", 2);
    }

    public void Attack()
    {
        Debug.Log("Attack !");
        var action = new BattleAction(BattleCommand.Attack);
        action.Target = new Cursor(Cursor.eTargetType.SINGLE_ENEMY, getActiveBattler(), Cursor.POSSIBLE_TARGETS_ONE, Actors, Enemies);
        ShowTargetChoice(action);
        lastCommand = BattleCommand.Attack;
    }

    private BattleCommand lastCommand;
    public void BackTarget()
    {
        RemoveTargetChoice();
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
        var lastAction = getActiveBattler().lastAction;
        Debug.LogFormat("LastCommand => {0} !", lastAction.Value);
        switch (lastAction.Value.Kind)
        {
            default:
            case BattleCommand.Nothing:
                break;
            case BattleCommand.Attack:
                Attack();
                break;
            case BattleCommand.Skills:
                //Skills();
                ShowTargetChoice(lastAction.Value);
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
        var skillManager = _currentEvent.GetComponent<SkillsManager>();
        skillManager.skills = getActiveBattler().skills;
        skillManager.OnClick += (sender, e) =>
        {
            var skill = (SkillData)sender;
            ShowTargetChoice(new BattleAction(BattleCommand.Skills, skill)
            {
                Target = new Cursor(Cursor.eTargetType.SINGLE_ENEMY, getActiveBattler(), skill.TargetsPossible, Actors, Enemies)
            });
        };
        skillManager.enabled = true;
        lastCommand = BattleCommand.Skills;
    }

    public void Items()
    {
        Debug.Log("Items !");
        lastCommand = BattleCommand.Items;
        RefreshButtons(false);
    }

    public void Run()
    {
        lastCommand = BattleCommand.Run;
        RefreshButtons(false);
        _currentEvent.Move(Direction.Back);
    }
}
