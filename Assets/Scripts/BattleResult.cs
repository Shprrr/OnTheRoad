using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BattleResult : MonoBehaviour
{
    private Animator animator;

    public CurrentEvent currentEvent;
    public Text titleText;
    public int moneyGained;
    public GameObject moneyTitle;
    public Text moneyText;
    public IItemData[] itemsGained;
    public GameObject itemsTitle;
    public GameObject itemsContent;
    public GameObject itemPrefab;

    public bool gameOver;
    public GameObject gameOverText;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        animator = GetComponent<Animator>();

        gameOverText.SetActive(gameOver);
        moneyTitle.SetActive(!gameOver);
        moneyText.gameObject.SetActive(!gameOver);
        itemsTitle.SetActive(!gameOver);

        currentEvent.party.money += moneyGained;
        moneyText.text = moneyGained.ToString("### ##0").Trim();

        itemsContent.DestroyAllChildren();
        if (itemsGained != null)
            foreach (var item in itemsGained)
            {
                currentEvent.party.AddItem(item);
                var go = Instantiate(itemPrefab, itemsContent.transform);
                go.GetComponent<Item>().data = item;
            }
    }

    public void ContinueClick()
    {
        animator.SetTrigger("close");
        if (gameOver)
        {
            currentEvent.party.FullHeal();
            currentEvent.AccessNextMap();
        }
    }
}
