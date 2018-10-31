using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ItemsManager : MonoBehaviour
{
    private Animator animator;

    public GameObject itemContent;
    public GameObject itemPrefab;
    public IItemData[] items;
    public Func<IItemData, bool> interactableItems;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        itemContent.DestroyAllChildren();
        foreach (var item in items)
        {
            var go = Instantiate(itemPrefab, itemContent.transform);
            go.GetComponent<Item>().data = item;
            go.GetComponent<Button>().onClick.AddListener(() => Click(item));
            go.GetComponent<Button>().interactable = interactableItems?.Invoke(item) ?? false;
        }
    }

    public event EventHandler OnClick;
    public void Click(IItemData item)
    {
        OnClick?.Invoke(item, EventArgs.Empty);
        animator.SetTrigger("close");
    }

    public void Back()
    {
        animator.SetTrigger("close");
    }
}
