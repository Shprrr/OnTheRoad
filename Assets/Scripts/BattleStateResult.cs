using UnityEngine;

public class BattleStateResult : StateMachineBehaviour
{
    public BattleEvent battle;
    public GameObject resultPanelPrefab;

    // Appelée sur la première image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        battle = (BattleEvent)animator.GetComponent<CurrentEvent>().currentEvent;

        var result = Instantiate(resultPanelPrefab, GameObject.Find("Canvas").transform).GetComponent<BattleResult>();
        result.party = animator.GetComponent<CurrentEvent>().party;
        result.moneyGained = battle.Enemies.Count * 2;
        result.itemsGained = new ItemData[] { ItemFactory.Build("potionHp1", 1), ItemFactory.Build("potionSp1", 2) };

        battle.FinishBattle();
    }
}
