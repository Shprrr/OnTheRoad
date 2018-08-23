using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    public TargetSelectionManager manager;
    public Battler user;
    public Battler target;
    public Cursor.eTargetType targetType;
    public BattleAction action;

    public void Click()
    {
        action.Target.TargetType = targetType;
        action.Target.SingleTarget = target;
        manager.ActionState(user, action);
        manager.RemoveTargetChoice();
    }
}
