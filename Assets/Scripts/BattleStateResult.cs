using UnityEngine;

public class BattleStateResult : StateMachineBehaviour
{
    public BattleEvent battle;
    public GameObject resultPanelPrefab;

    // Appelée sur la première image où Update est exécuté, quand un statemachine évalue cet état
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        var currentEvent = animator.GetComponent<CurrentEvent>();
        battle = (BattleEvent)currentEvent.currentEvent;

        var result = Instantiate(resultPanelPrefab, currentEvent.canvas.transform).GetComponent<BattleResult>();
        result.currentEvent = currentEvent;
        result.gameOver = battle.Result != BattleEvent.BattleResult.WIN;
        if (!result.gameOver)
        {
            result.moneyGained = battle.Enemies.Count * 2;
            result.itemsGained = new IItemData[] { ItemFactory.Build("potionHp1", 1), ItemFactory.Build("potionSp1", 2) };

            battle.FinishBattle();
        }
    }
}
