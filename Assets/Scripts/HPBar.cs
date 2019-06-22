using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HPBar : MonoBehaviour
{
    private Slider slider;

    public Battler battler;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        slider.value = battler.Hp;
        slider.maxValue = battler.GetMaxHP();
        var fillImage = slider.fillRect.GetComponent<Image>();
        // From 100% to 50%, red goes from 0 to 255.
        // From 50% to 0%, green goes from 255 to 0.
        if (slider.value / slider.maxValue > 0.5f)
            fillImage.color = Color.Lerp(Color.yellow, Color.green, (slider.value / slider.maxValue - 0.5f) * 2);
        else
            fillImage.color = Color.Lerp(Color.red, Color.yellow, slider.value / slider.maxValue * 2);
    }
}
