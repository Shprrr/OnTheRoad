using UnityEngine;

[RequireComponent(typeof(Map))]
public class GreenHillMapGenerator : MonoBehaviour, IMapGenerator
{
    private Map map;

    public GameObject[] enemiesPrefabs;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        map = GetComponent<Map>();
    }

    public void Generate(int difficulty)
    {
        map.mapData.Clear();
        map.mapData.Add(new MapPosition(0, 0), new SpawnEvent());
        map.mapData.Add(new MapPosition(0, 1), new MapEvent());
        map.mapData.Add(new MapPosition(0, 2), new EndMapEvent());
    }
}
