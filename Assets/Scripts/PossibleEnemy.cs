using UnityEngine;
using UnityEngine.UI;

public class PossibleEnemy : MonoBehaviour
{
    public Text nameText;
    public PossibleEnemyData data;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        Update();
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        nameText.text = data?.EnemyName;
    }
}
