using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static BattleAction;

[RequireComponent(typeof(CurrentEvent))]
public class TargetSelectionManager : MonoBehaviour
{
    private CurrentEvent currentEvent;
    private List<GameObject> targetsPossible = new List<GameObject>();
    private bool wasDirectionActive, wasBattleCommandsActive;

    public GameObject targetPrefab;
    public Text attackName;
    public Text description;

    public List<Battler> Actors;
    public List<Battler> Enemies;

    public UnityAction onTargetSelected;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        currentEvent = GetComponent<CurrentEvent>();
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        Actors = new List<Battler>(currentEvent.party.GetComponentsInChildren<Battler>());
        Enemies = new List<Battler>();
    }

    public void ShowTargetChoice(Battler user, BattleAction action)
    {
        switch (action.Kind)
        {
            default:
            case BattleCommand.Nothing:
                attackName.text = "";
                description.text = "";
                break;
            case BattleCommand.Attack:
                attackName.text = "Attack";
                description.text = "Attack with weapon in hand.";
                break;
            case BattleCommand.Skills:
                attackName.text = action.Data.Name;
                description.text = action.Data.Description;
                break;
            case BattleCommand.Items:
                attackName.text = action.Data.Name;
                description.text = action.Data.Description;
                break;
            case BattleCommand.Run:
                attackName.text = "Run";
                description.text = "Escape battle and return to previous room.";
                break;
        }

        wasDirectionActive = currentEvent.directions.activeSelf;
        wasBattleCommandsActive = currentEvent.battleCommands.activeSelf;
        currentEvent.directions.SetActive(false);
        currentEvent.battleCommands.SetActive(false);
        currentEvent.targetSelection.SetActive(true);

        bool multi = false, all = false;
        var targets = new List<Battler>();
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
            var targetGO = Instantiate(targetPrefab, targetPossible.transform);
            var animator = targetGO.GetComponentInChildren<Animator>();
            animator.SetBool("multi", multi);
            animator.SetBool("all", all);
            animator.enabled = true;
            targetsPossible.Add(targetGO);

            var target = targetGO.GetComponent<TargetSelection>();
            target.user = user;
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
            target.manager = this;
        }
    }

    public void RemoveTargetChoice()
    {
        currentEvent.targetSelection.SetActive(false);
        currentEvent.directions.SetActive(wasDirectionActive);
        currentEvent.battleCommands.SetActive(wasBattleCommandsActive);

        foreach (var target in targetsPossible)
        {
            Destroy(target);
        }
        targetsPossible.Clear();
    }

    public void BackTarget()
    {
        RemoveTargetChoice();
    }

    public void ActionState(Battler user, BattleAction action)
    {
        var targets = action.Target.getTargetBattler();
        switch (action.Kind)
        {
            case BattleCommand.Attack:
                for (int i = 0; i < targets.Count; i++)
                    if (targets[i] != null)
                        user.Attacks(targets[i]);
                break;

            case BattleCommand.Skills:
                {
                    int skillLevel;
                    if (!user.Casts((SkillData)action.Data, out skillLevel))
                        throw new System.InvalidOperationException("Not enough SP.");

                    int nbTarget = 0;
                    for (int i = 0; i < targets.Count; i++)
                        if (targets[i] != null && !targets[i].CantFight)
                            nbTarget++;

                    for (int i = 0; i < targets.Count; i++)
                        if (targets[i] != null && !targets[i].CantFight)
                            user.Used(targets[i], action.Data, nbTarget);
                }
                break;
            //TODO: Revoir nbTarget vs CantFight avec des items qui target les morts
            case BattleCommand.Items:
                {
                    int nbTarget = 0;
                    for (int i = 0; i < targets.Count; i++)
                        if (targets[i] != null && !targets[i].CantFight)
                            nbTarget++;

                    for (int i = 0; i < targets.Count; i++)
                    {
                        if (targets[i] != null && !targets[i].CantFight)
                            user.Used(targets[i], action.Data, nbTarget);
                    }

                    if (user.IsPlayer)
                        currentEvent.party.DropItem((IItemData)action.Data);
                }
                break;

            case BattleCommand.Run:
                break;

            case BattleCommand.Nothing:
                break;
        }
        user.lastAction = action;
        onTargetSelected?.Invoke();
    }
}
