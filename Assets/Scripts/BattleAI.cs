using System.Collections.Generic;
using UnityEngine;
using static BattleAction;

[RequireComponent(typeof(Battler))]
public class BattleAI : MonoBehaviour
{
    /// <summary>
    /// Decide what action the AI will take.
    /// </summary>
    /// <param name="actors">AI party</param>
    /// <param name="enemies">Enemies to the AI</param>
    /// <returns></returns>
    public BattleAction ChooseAction(List<Battler> actors, List<Battler> enemies)
    {
        var action = new BattleAction();
        var indexTargetPotential = new List<int>();

        //TODO: Si aucun skill appris, attack obligatoirement physique.
        action.Kind = BattleCommand.Attack;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null && !enemies[i].CantFight)
                indexTargetPotential.Add(i);
        }
        //var cursor = new Cursor(Cursor.eTargetType.SINGLE_PARTY, indexTargetPotential[Random.Range(0, indexTargetPotential.Count)], Cursor.POSSIBLE_TARGETS_ANYONE);
        var cursor = new Cursor(Cursor.eTargetType.SINGLE_PARTY, GetComponent<Battler>(), Cursor.POSSIBLE_TARGETS_ANYONE, enemies, actors);
        cursor.SingleTarget = enemies[indexTargetPotential[Random.Range(0, indexTargetPotential.Count)]];
        action.Target = cursor;
        return action;

#if false
        /*
         * Si l'attack physique est meilleur que l'attack magic et
         *  que le target n'est pas résistant à l'attack,
         * Kind = Attack
         * Target = targetRandomParmisCeuxPossible
        */
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
                continue;

            //// Calculer le potentiel de dommage.
            //int baseDamage = getBaseDamage(eDamageOption.RIGHT) - enemy.getDefenseDamage();
            //int hitPourc = getHitPourc(eDamageOption.RIGHT) - enemy.getEvadePourc();
            //int multi = getAttackMultiplier() - enemy.getDefenseMultiplier();

            //// Réquilibrer les valeurs out of range.
            //if (baseDamage < 1)
            //    baseDamage = 1;

            //if (hitPourc < 0)
            //    hitPourc = 0;

            //if (multi < 0)
            //    multi = 0;

            //// ???

            ////TODO: Si Weapon équipé main gauche.
            //int attPhysic = getBaseDamage(ePhysicalDamageOption.RIGHT) - enemies[i].getDefenseDamage();
            //int attMagic = 0;

            //if (attPhysic > attMagic)
            //{
            //    action.Kind = BattleAction.eKind.ATTACK;
            //    indexTargetPotential.Add(i);
            //}
        }

        if (action.Kind == BattleAction.eKind.ATTACK)
        {
            //action.Target = new Cursor(game, enemies, actors, eTargetType.SINGLE_PARTY,
            //    indexTargetPotential[Extensions.rand.Next(indexTargetPotential.Count)]);
            return action;
        }

        /*
         * Si on n'attaque pas physique, regarder la possibilité d'utiliser les skills appris.
         * Kind = Magic
         * Target = targetRandomParmisCeuxPossible
         * skillId = SkillIdChoisi
        */
        //TODO: Parcourir les skills appris.
        //Changer le rank.

        /*
         * S'il n'y pas de skill utile,
         * Kind = Defend
        */
        action.Kind = BattleAction.eKind.DEFEND;

        return action;
#endif
    }
}
