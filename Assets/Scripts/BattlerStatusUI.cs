using UnityEngine;
using UnityEngine.UI;

public class BattlerStatusUI : MonoBehaviour
{
    public BattlerStatus battlerStatus;

    public int indexShown;
    public float secondsWaitCycle;
    private float timeWaiting;
    public Text nameText;
    public Text turnLeft;
    public Text page;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        battlerStatus = GetComponent<BattlerStatus>();
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        if (battlerStatus.ActiveStatuses.Count > 0)
        {
            if (indexShown >= battlerStatus.ActiveStatuses.Count) indexShown = 0;

            if (nameText != null) nameText.text = battlerStatus.ActiveStatuses[indexShown].ToString();
            if (turnLeft != null) turnLeft.text = battlerStatus.ActiveStatuses[indexShown].TurnLeft == -1 ? "" : battlerStatus.ActiveStatuses[indexShown].TurnLeft.ToString();
            if (page != null) page.text = (indexShown + 1) + "/" + battlerStatus.ActiveStatuses.Count;
            timeWaiting += Time.unscaledDeltaTime;
            if (timeWaiting >= secondsWaitCycle)
            {
                indexShown++;
                if (indexShown >= battlerStatus.ActiveStatuses.Count)
                    indexShown = 0;
                timeWaiting = 0;
            }
        }
        else
        {
            if (nameText != null) nameText.text = "None";
            if (turnLeft != null) turnLeft.text = "";
            if (page != null) page.text = "";
            timeWaiting = 0;
        }
    }
}
