using UnityEngine;

[RequireComponent(typeof(CurrentEvent))]
public class MoveInMap : MonoBehaviour
{
    CurrentEvent currentEvent;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        currentEvent = GetComponent<CurrentEvent>();
    }

    public void Move(MapDirection direction)
    {
        currentEvent.Move(direction.direction);
    }
}
