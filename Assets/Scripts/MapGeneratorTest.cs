using UnityEngine;

[RequireComponent(typeof(Map))]
public class MapGeneratorTest : MonoBehaviour
{
    private Map map;

    public GameObject[] enemiesPrefabs = new GameObject[2];

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        map = GetComponent<Map>();
    }

    public void Generate()
    {
        map.mapData.Clear();
        map.mapData.Add(new MapPosition(0, 0), new MapEvent { Type = "Spawn" });
        map.mapData.Add(new MapPosition(-1, 0), new MapEvent { Type = "Treasure" });
        map.mapData.Add(new MapPosition(1, 0), new MapEvent { Type = "Treasure" });
        map.mapData.Add(new MapPosition(0, 1), new BattleEvent { Type = "Battle", prefabEnemy1 = enemiesPrefabs[0], prefabEnemy2 = enemiesPrefabs[1] });
        map.mapData.Add(new MapPosition(0, 2), new MapEvent { Type = "Boss" });

        map.mapData.Add(new MapPosition(2, 0), new MapEvent());
        map.mapData.Add(new MapPosition(2, 1), new MapEvent());
        map.mapData.Add(new MapPosition(2, -1), new MapEvent());
        map.mapData.Add(new MapPosition(2, -2), new MapEvent());
        map.mapData.Add(new MapPosition(3, 1), new MapEvent());
        map.mapData.Add(new MapPosition(3, 0), new MapEvent());
        map.mapData.Add(new MapPosition(3, -1), new MapEvent());
        map.mapData.Add(new MapPosition(3, -2), new MapEvent());
        map.mapData.Add(new MapPosition(3, -3), new MapEvent());
        map.mapData.Add(new MapPosition(4, -2), new MapEvent());
        map.mapData.Add(new MapPosition(4, -1), new MapEvent());
        map.mapData.Add(new MapPosition(4, 0), new MapEvent());
        map.mapData.Add(new MapPosition(4, 1), new MapEvent());
        map.mapData.Add(new MapPosition(4, 2), new MapEvent());
        map.mapData.Add(new MapPosition(4, 3), new MapEvent());
        map.mapData.Add(new MapPosition(4, 4), new MapEvent());
        map.mapData.Add(new MapPosition(3, 3), new MapEvent());
        map.mapData.Add(new MapPosition(5, 3), new MapEvent());
        map.mapData.Add(new MapPosition(5, 1), new MapEvent());
        map.mapData.Add(new MapPosition(5, 0), new MapEvent());
        map.mapData.Add(new MapPosition(5, -1), new MapEvent());
        map.mapData.Add(new MapPosition(5, -2), new MapEvent());
    }
}
