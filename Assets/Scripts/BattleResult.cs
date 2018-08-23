using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BattleResult : MonoBehaviour
{
    private Animator animator;

    public Party party;
    public int moneyGained;
    public Text moneyText;
    public ItemData[] itemsGained;
    public GameObject itemsContent;
    public GameObject itemPrefab;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        animator = GetComponent<Animator>();

        party.money += moneyGained;
        moneyText.text = moneyGained.ToString("### ##0").Trim();

        itemsContent.DestroyAllChildren();
        foreach (var item in itemsGained)
        {
            party.AddItem(item);
            var go = Instantiate(itemPrefab, itemsContent.transform);
            go.GetComponent<Item>().data = item;
        }
    }

    public void ContinueClick()
    {
        animator.SetTrigger("close");
    }
}
