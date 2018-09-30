using UnityEngine;
using UnityEngine.UI;

public class MapTemplate : MonoBehaviour
{
    public Text nameText;
    public MapTemplateData data;

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        nameText.text = data?.Name;
    }
}
