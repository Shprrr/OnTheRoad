using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Battler))]
public class BattlerStatsUI : MonoBehaviour
{
    private Battler battler;

    public Text currentHP;
    public Text maxHP;
    public Text currentSP;
    public Text maxSP;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        battler = GetComponent<Battler>();
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        currentHP.text = battler.CurrentHP.ToString("### ##0").Trim();
        maxHP.text = battler.MaxHP.ToString("### ##0").Trim();
        currentSP.text = battler.CurrentSP.ToString("##0");
        maxSP.text = battler.MaxSP.ToString("##0");
    }
}
