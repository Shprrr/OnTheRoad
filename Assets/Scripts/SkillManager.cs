using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject skillPanel;
    private GameObject skillContent;
    public GameObject skillPrefab;
    public SkillData[] skills;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        skillContent = skillPanel.GetComponentInChildren<ContentSizeFitter>().gameObject;
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        enabled = false;
    }

    // Cette fonction est appelée quand l'objet est activé et actif
    private void OnEnable()
    {
        skillContent.DestroyAllChildren();
        foreach (var skill in skills)
        {
            var go = Instantiate(skillPrefab, skillContent.transform);
            go.GetComponent<Skill>().data = skill;
            go.GetComponent<Button>().onClick.AddListener(() => Click(skill));
        }
        skillPanel.SetActive(true);
    }

    // Cette fonction est appelée quand le comportement est désactivé ou inactif
    private void OnDisable()
    {
        OnClick = null;
        if (skillPanel != null)
            skillPanel.SetActive(false);
    }

    public event EventHandler OnClick;
    public void Click(SkillData skill)
    {
        OnClick?.Invoke(skill, EventArgs.Empty);
        enabled = false;
    }

    public void Back()
    {
        enabled = false;
    }
}
