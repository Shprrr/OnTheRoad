using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour, ISerializationCallbackReceiver
{
    [Serializable]
    public class MapPositionEvent
    {
        public MapPosition _mapPosition;
        public MapEvent _mapEvent;

        public MapPositionEvent(MapPosition mapPosition, MapEvent mapEvent)
        {
            _mapPosition = mapPosition;
            _mapEvent = mapEvent;
        }

        public override string ToString()
        {
            return _mapPosition.ToString();
        }
    }
    public List<MapPositionEvent> _data;

    public GameObject currentEvent;

    // Unity doesn't know how to serialize a Dictionary
    public Dictionary<MapPosition, MapEvent> mapData = new Dictionary<MapPosition, MapEvent>();

    public void OnBeforeSerialize()
    {
        _data.Clear();

        foreach (var kvp in mapData)
        {
            _data.Add(new MapPositionEvent(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        mapData = new Dictionary<MapPosition, MapEvent>();

        for (int i = 0; i < _data.Count; i++)
        {
            mapData.Add(_data[i]._mapPosition, _data[i]._mapEvent);
        }
    }

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        _data = new List<MapPositionEvent>();
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        Generate(GetComponent<MapGeneratorTest>());
    }

    public void Generate(IMapGenerator generator, int difficulty = 1)
    {
        if (generator == null) return;

        generator.Generate(difficulty);
        var position = mapData.FirstOrDefault(m => m.Value.Type == "Spawn").Key;
        var eventComponent = currentEvent.GetComponent<CurrentEvent>();
        eventComponent.Move(position);
    }

    public bool IsVisited(MapPosition position, bool checkNoRoom = true)
    {
        if (mapData.ContainsKey(position))
            return mapData[position].Visited;

        if (!checkNoRoom) return false;

        // If no room, checks if near rooms are visited.
        bool leftVisited = IsVisited(new MapPosition(position.X - 1, position.Y), false);
        if (leftVisited) return leftVisited;
        bool rightVisited = IsVisited(new MapPosition(position.X + 1, position.Y), false);
        if (rightVisited) return rightVisited;
        bool upVisited = IsVisited(new MapPosition(position.X, position.Y + 1), false);
        if (upVisited) return upVisited;
        bool downVisited = IsVisited(new MapPosition(position.X, position.Y - 1), false);
        return downVisited;
    }
}
