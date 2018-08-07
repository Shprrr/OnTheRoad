using UnityEngine;

[RequireComponent(typeof(Battler))]
public class SpawnSlider : MonoBehaviour
{
    public GameObject sliderPrefab;
    public Transform sliderPosition;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        var go = Instantiate(sliderPrefab, sliderPosition);
        go.GetComponent<HPBar>().battler = GetComponent<Battler>();
    }
}
