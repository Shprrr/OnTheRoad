using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Chest : MonoBehaviour
{
    private Animator animator;

    public TreasureEvent currentEvent;
    public bool open;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        animator.SetBool("open", open);
        animator.SetFloat("reverseAnim", open ? 1 : -1);
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        UpdateAnimator();
    }

    public void Open()
    {
        open = !open;
        currentEvent.Open = open;
    }
}
