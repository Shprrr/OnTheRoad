using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text nameText;
    public Text amountText;
    public IItemData data;

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        nameText.text = data?.Name;
        amountText.text = string.IsNullOrEmpty(data?.Id) ? "" : data?.Amount.ToString();
    }
}
