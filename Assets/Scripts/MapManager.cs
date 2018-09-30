using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SwipeControl))]
public class MapManager : MonoBehaviour
{
    private SwipeControl swipe;

    public Animator animator;
    public CurrentEvent currentEvent;
    public RoomTileset roomTileSet;
    public GameObject roomPrefab;
    public RectTransform roomContent;

    /// <summary>
    /// Count from center.
    /// </summary>
    public int xCount = 2, yCount = 2;
    private GameObject[,] rooms;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        swipe = GetComponent<SwipeControl>();
        rooms = new GameObject[xCount * 2 + 1, yCount * 2 + 1];
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        roomContent.gameObject.DestroyAllChildren();
        for (int y = 0; y < yCount * 2 + 1; y++)
        {
            for (int x = 0; x < xCount * 2 + 1; x++)
            {
                rooms[x, y] = Instantiate(roomPrefab, roomContent);
                if (x == xCount && y == yCount)
                    rooms[x, y].transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
        }
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        if (swipe.swipeUp)
            animator.SetBool("open", true);

        if (swipe.swipeDown)
            animator.SetBool("open", false);

        GenerateMap();
    }

    private void GenerateMap()
    {
        //Debug.LogFormat("{0}, {1}", (currentEvent.currentPosition.X - xCount) - currentEvent.currentPosition.X + xCount, (currentEvent.currentPosition.Y - yCount) - currentEvent.currentPosition.Y + yCount);
        //var watch = new System.Diagnostics.Stopwatch();
        //watch.Start();
        for (int y = currentEvent.currentPosition.Y - yCount; y < currentEvent.currentPosition.Y + yCount + 1; y++)
        {
            for (int x = currentEvent.currentPosition.X - xCount; x < currentEvent.currentPosition.X + xCount + 1; x++)
            {
                var roomGO = rooms[x - currentEvent.currentPosition.X + xCount, y - currentEvent.currentPosition.Y + yCount];
                roomGO.name = "Room " + x + "," + y;

                if (!currentEvent.map.IsVisited(new MapPosition(x, y)))
                {
                    roomGO.GetComponent<Image>().sprite = roomTileSet.fogOfWar;
                    continue;
                }

                if (!currentEvent.map.mapData.ContainsKey(new MapPosition(x, y)))
                {
                    roomGO.GetComponent<Image>().sprite = roomTileSet.noRoom;
                    continue;
                }

                bool leftDoor = currentEvent.map.mapData.ContainsKey(new MapPosition(x - 1, y));
                bool rightDoor = currentEvent.map.mapData.ContainsKey(new MapPosition(x + 1, y));
                bool upDoor = currentEvent.map.mapData.ContainsKey(new MapPosition(x, y + 1));
                bool downDoor = currentEvent.map.mapData.ContainsKey(new MapPosition(x, y - 1));
                roomGO.GetComponent<Image>().sprite = roomTileSet.GetDoorSprite(leftDoor, rightDoor, upDoor, downDoor);
            }
        }
        //watch.Stop();
        //Debug.Log(watch.Elapsed);
    }
}
