using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    public BattleEvent battle;
    public Battler target;
    public Cursor.eTargetType targetType;
    public BattleAction action;

    public void Click()
    {
        action.Target.TargetType = targetType;
        action.Target.SingleTarget = target;
        battle.ActionState(action);
        battle.RemoveTargetChoice();
    }
}
