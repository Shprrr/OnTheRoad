using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEvent : MonoBehaviour
{
    public CTBManager ctbManager;

    public Map map;

    public MapPosition currentPosition;
    public MapEvent currentEvent;

    public GameObject party;

    public GameObject directions;
    public GameObject buttonLeft;
    public GameObject buttonMiddle;
    public GameObject buttonRight;
    public GameObject buttonBack;

    public GameObject battleCommands;
    public GameObject buttonAttack;
    public GameObject buttonLastCommand;
    public GameObject buttonSkills;
    public GameObject buttonItems;
    public GameObject buttonRun;
    public GameObject buttonBackTarget;
    public GameObject ctbPanel;

    public GameObject enemySpawn1;
    public GameObject enemySpawn2;
    public GameObject enemySpawn3;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        ctbManager = GetComponent<CTBManager>();
    }

    public void Move(Direction direction)
    {
        MapPosition nextPosition = new MapPosition(currentPosition.X, currentPosition.Y);

        switch (direction)
        {
            case Direction.Back:
                nextPosition.Y -= 1;
                break;
            case Direction.Left:
                nextPosition.X -= 1;
                break;
            case Direction.Middle:
                nextPosition.Y += 1;
                break;
            case Direction.Right:
                nextPosition.X += 1;
                break;
        }

        Move(nextPosition);
    }

    public void Move(MapPosition position)
    {
        if (!map.mapData.ContainsKey(position))
            throw new KeyNotFoundException("Wrong direction.");

        currentPosition = position;
        currentEvent = map.mapData[position];

        battleCommands.SetActive(false);
        buttonAttack.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonLastCommand.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonSkills.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonItems.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonRun.GetComponent<Button>().onClick.RemoveAllListeners();
        ctbPanel.SetActive(false);
        GetComponent<Animator>().enabled = false;

        enemySpawn1.DestroyAllChildren();
        enemySpawn2.DestroyAllChildren();
        enemySpawn3.DestroyAllChildren();

        RefreshDirections();
        currentEvent.RefreshEvent(this);
    }

    public void RefreshDirections()
    {
        directions.SetActive(true);
        buttonLeft.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X - 1, currentPosition.Y)));
        buttonMiddle.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X, currentPosition.Y + 1)));
        buttonRight.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X + 1, currentPosition.Y)));
        buttonBack.SetActive(map.mapData.ContainsKey(new MapPosition(currentPosition.X, currentPosition.Y - 1)));
    }
}
