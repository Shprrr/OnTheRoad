using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SkillsManager : MonoBehaviour
{
    private Animator animator;

    public GameObject skillContent;
    public GameObject skillPrefab;
    public Battler user;
    public SkillData[] skills;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        skillContent.DestroyAllChildren();
        foreach (var skill in skills)
        {
            var go = Instantiate(skillPrefab, skillContent.transform);
            go.GetComponent<Skill>().data = skill;
            go.GetComponent<Button>().interactable = user.Sp >= skill.SpCost;
            go.GetComponent<Button>().onClick.AddListener(() => Click(skill));
        }
    }

    public event EventHandler OnClick;
    public void Click(SkillData skill)
    {
        OnClick?.Invoke(skill, EventArgs.Empty);
        animator.SetTrigger("close");
    }

    public void Back()
    {
        animator.SetTrigger("close");
    }
}
