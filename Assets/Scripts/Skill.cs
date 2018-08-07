using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Text nameText;
    public Text spCostText;
    public SkillData data;

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        nameText.text = data?.Name;
        spCostText.text = data?.SpCost.ToString();
    }
}
