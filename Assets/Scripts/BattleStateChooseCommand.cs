using System.Collections.Generic;
using UnityEngine;

public class BattleStateChooseCommand : StateMachineBehaviour
{
    public BattleEvent battle;

    // Appelée sur la première image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        battle = (BattleEvent)animator.GetComponent<CurrentEvent>().currentEvent;

        // Determine win/loss situation
        if (battle.Judge())
        {
            // If won or lost: end method
            animator.SetBool("end", true);
            return;
        }

        battle._currentEvent.ctbManager.BeginTurn(battle);

        Battler activeBattler = battle.GetActiveBattler();
        activeBattler.BattlerStatus.PassTurn();

        var hpRegen = activeBattler.GetCalculatedStat(CharacteristicFactory.HPRegenId);
        hpRegen += activeBattler.GetCalculatedStat(CharacteristicFactory.HPRegenRateId) * activeBattler.GetMaxHP();
        if (hpRegen <= -1 || hpRegen >= 1)
            activeBattler.SelfTakingDamage(new Damage(Damage.DamageType.HP, (int)-hpRegen, 1, activeBattler, activeBattler), "AnimationAttack1");

        var spRegen = activeBattler.GetCalculatedStat(CharacteristicFactory.SPRegenId);
        spRegen += activeBattler.GetCalculatedStat(CharacteristicFactory.SPRegenRateId) * activeBattler.GetMaxSP();
        if (spRegen <= -1 || spRegen >= 1)
            activeBattler.SelfTakingDamage(new Damage(Damage.DamageType.MP, (int)-spRegen, 1, activeBattler, activeBattler), "AnimationAttack1");

        //if (activeBattler.CantFight)
        //{
        //    // Active battler can't choose an action so we jump directly to the show damage state to apply changes in statuses.
        //    Debug.LogFormat("DoNothing for {0}", activeBattler.name);
        //    battle._currentEvent.targetSelectionManager.ActionState(activeBattler, new BattleAction());
        //    return;
        //}

        //TODO: Bug potentiel restriction plus DoT qui tue
        if (activeBattler.BattlerStatus.GetRestriction().HasValue)
        {
            var restriction = activeBattler.BattlerStatus.GetRestriction().Value;
            Debug.LogFormat("Restricted {1} for {0}", activeBattler.name, restriction);

            var action = new BattleAction();
            var indexTargetPotential = new List<int>();
            var actors = activeBattler.IsPlayer ? battle.Actors : battle.Enemies;
            var enemies = activeBattler.IsPlayer ? battle.Enemies : battle.Actors;
            switch (restriction)
            {
                case RestrictionType.CannotMove:
                    break;

                case RestrictionType.AttackAlly:
                    action.Kind = BattleAction.BattleCommand.Attack;

                    for (int i = 0; i < actors.Count; i++)
                    {
                        if (actors[i] != null && !actors[i].CantFight)
                            indexTargetPotential.Add(i);
                    }

                    action.Target = new Cursor(Cursor.eTargetType.SINGLE_PARTY, activeBattler, Cursor.POSSIBLE_TARGETS_ANYONE, actors, enemies)
                    {
                        SingleTarget = actors[indexTargetPotential[Random.Range(0, indexTargetPotential.Count)]]
                    };
                    break;

                case RestrictionType.AttackEveryone:
                    action.Kind = BattleAction.BattleCommand.Attack;

                    for (int i = 0; i < actors.Count; i++)
                    {
                        if (actors[i] != null && !actors[i].CantFight)
                            indexTargetPotential.Add(i);
                    }

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i] != null && !enemies[i].CantFight)
                            indexTargetPotential.Add(actors.Count + i);
                    }

                    action.Target = new Cursor(Cursor.eTargetType.SINGLE_PARTY, activeBattler, Cursor.POSSIBLE_TARGETS_ANYONE, actors, enemies);
                    var targetIndex = indexTargetPotential[Random.Range(0, indexTargetPotential.Count)];
                    if (targetIndex < actors.Count)
                        action.Target.SingleTarget = actors[targetIndex];
                    else
                        action.Target.SingleTarget = enemies[targetIndex - actors.Count];
                    break;

                case RestrictionType.AttackEnemy:
                    action.Kind = BattleAction.BattleCommand.Attack;

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i] != null && !enemies[i].CantFight)
                            indexTargetPotential.Add(i);
                    }

                    action.Target = new Cursor(Cursor.eTargetType.SINGLE_ENEMY, activeBattler, Cursor.POSSIBLE_TARGETS_ANYONE, actors, enemies)
                    {
                        SingleTarget = enemies[indexTargetPotential[Random.Range(0, indexTargetPotential.Count)]]
                    };
                    break;
            }
            battle._currentEvent.targetSelectionManager.ActionState(activeBattler, action);
            return;
        }

        animator.SetInteger("state", 1);
    }
}
