using System;
using System.Collections.Generic;
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
        var generator = GetComponent<MapGeneratorTest>();
        if (generator != null)
        {
            generator.Generate();
            var eventComponent = currentEvent.GetComponent<CurrentEvent>();
            eventComponent.Move(new MapPosition(0, 0));
        }
    }
}
