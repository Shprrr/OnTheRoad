using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class NextMapManager : MonoBehaviour
{
    private Animator animator;

    public Party party;
    public MapTemplateData[] templates;
    public GameObject mapTemplatePrefab;
    public RectTransform templatesContent;

    [Header("Map Selection")]
    public GameObject mapSelectionGroup;
    public Text mapName;
    public Text mapDifficulty;
    public Button lowerDifficulty;
    public Button raiseDifficulty;
    public GameObject possibleEnemyPrefab;
    public RectTransform possibleEnemiesContent;
    public Text money;
    public Text cost;
    public Button buttonGo;

    private MapTemplateData mapSelected;
    private int difficulty;
    private int Cost { get { return mapSelected.BaseCost + mapSelected.CostByDifficulty * difficulty; } }
    public Map mapGenerator;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < templates.Length; i++)
        {
            templates[i] = MapTemplateFactory.Build(templates[i].Id);
        }
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        templatesContent.gameObject.DestroyAllChildren();
        foreach (var map in templates)
        {
            var mapGO = Instantiate(mapTemplatePrefab, templatesContent);
            mapGO.GetComponent<Button>().onClick.AddListener(() => MapSelection(map));
            mapGO.GetComponent<MapTemplate>().data = map;
        }
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        mapSelectionGroup.SetActive(mapSelected != null);
        if (mapSelected == null) return;

        mapName.text = mapSelected.Name;
        mapDifficulty.text = difficulty.ToString();
        lowerDifficulty.interactable = difficulty > mapSelected.MinDifficulty;
        raiseDifficulty.interactable = difficulty < mapSelected.MaxDifficulty;
        money.text = party.money.ToString("### ##0").Trim();
        cost.text = Cost.ToString("### ##0").Trim();
        buttonGo.interactable = Cost <= party.money;
    }

    public void MapSelection(MapTemplateData map)
    {
        mapSelected = map;
        difficulty = mapSelected.MinDifficulty;

        possibleEnemiesContent.gameObject.DestroyAllChildren();
        foreach (var enemy in map.PossibleEnemiesData)
        {
            var enemyGO = Instantiate(possibleEnemyPrefab, possibleEnemiesContent);
            enemyGO.GetComponent<PossibleEnemy>().data = enemy;
        }
    }

    public void RaiseDifficulty()
    {
        if (difficulty < mapSelected.MaxDifficulty)
            difficulty++;
    }

    public void LowerDifficulty()
    {
        if (difficulty > mapSelected.MinDifficulty)
            difficulty--;
    }

    public void Go()
    {
        party.money -= Cost;
        mapGenerator.Generate(mapGenerator.GetComponent(mapSelected.Generator) as IMapGenerator, difficulty);
        animator.SetTrigger("close");
    }
}
