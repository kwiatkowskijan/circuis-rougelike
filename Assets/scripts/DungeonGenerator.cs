using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject startingRoom;
    private GameObject player;
    public GameObject meleEnemyPrefab;
    public int numberOfRooms = 15;
    public float roomDistanceX = 25f;
    public float roomDistanceY = 15f;
    public int maxRoomsPerIteration = 4;
    private List<GameObject> generatedRooms = new List<GameObject>();
    private Dictionary<GameObject, int> enemiesInRoom = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, bool> roomsSpawned = new Dictionary<GameObject, bool>();
    public LayerMask roomLayer;
    private float MaxConDistanceX = 25f;
    private float MaxConDistanceY = 15f;

    public Vector3Int coordsDoorsUp1;
    public Vector3Int coordsDoorsUp2;
    public Vector3Int coordsDoorsDown1;
    public Vector3Int coordsDoorsDown2;
    public Vector3Int coordsDoorsRight1;
    public Vector3Int coordsDoorsRight2;
    public Vector3Int coordsDoorsLeft1;
    public Vector3Int coordsDoorsLeft2;
    public TileBase doorUp1;
    public TileBase doorUp2;
    public TileBase doorDown1;
    public TileBase doorDown2;
    public TileBase doorRight1;
    public TileBase doorRight2;
    public TileBase doorLeft1;
    public TileBase doorLeft2;

    public GameObject ChangeLevelUp;
    public GameObject ChangeLevelLeft;
    public GameObject ChangeLevelRight;
    public GameObject ChangeLevelDown;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GenerateDungeon();
    }

    void Update()
    {
        CheckPlayerRoom();
    }

    void GenerateDungeon()
    {
        if (startingRoom == null)
        {
            Debug.LogWarning("Brak pocz¹tkowego pomieszczenia do generowania kolejnych.");
            return;
        }

        generatedRooms.Add(startingRoom);

        int generatedCount = 0;

        while (generatedCount < numberOfRooms - 1)
        {
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
            Vector2 randomDirection = directions[Random.Range(0, directions.Length)];

            Vector3 newPosition = generatedRooms[Random.Range(0, generatedRooms.Count)].transform.position +
                new Vector3(randomDirection.x * roomDistanceX, randomDirection.y * roomDistanceY, 0f);

            bool positionOccupied = IsPositionOccupied(newPosition);

            if (!positionOccupied)
            {
                string roomName = "Room " + (generatedCount + 2);

                GameObject newRoom = Instantiate(roomPrefab, newPosition, Quaternion.identity);
                newRoom.name = roomName;

                generatedRooms.Add(newRoom);
                generatedCount++;
            }
        }

        bool IsPositionOccupied(Vector3 position)
        {
            foreach (GameObject room in generatedRooms)
            {
                if (Vector3.Distance(room.transform.position, position) < 0.1f)
                {
                    return true;
                }
            }
            return false;
        }
    }


    void CheckPlayerRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        int LayerDungeon = LayerMask.NameToLayer("Dungeon");


        if (player != null)
        {
            foreach (GameObject room in generatedRooms)
            {
                BoxCollider2D floorCollider = room.GetComponentInChildren<BoxCollider2D>();

                if (floorCollider != null && floorCollider.bounds.Contains(player.transform.position))
                {
                    Transform gridTransform = room.transform.Find("Grid");

                    if (gridTransform != null)
                    {
                        Transform floorTransform = gridTransform.Find("Floor");

                        if (floorTransform != null)
                        {
                            GameObject floorObject = floorTransform.gameObject;
                            floorObject.layer = LayerIgnoreRaycast;
                        }
                    }

                    if (!roomsSpawned.ContainsKey(room) || !roomsSpawned[room])
                    {
                        SpawnEnemyInRoom(room);
                        roomsSpawned[room] = true;
                    }
                }
                else 
                {
                    Transform gridTransform = room.transform.Find("Grid");

                    if (gridTransform != null)
                    {
                        Transform floorTransform = gridTransform.Find("Floor");

                        if (floorTransform != null)
                        {
                            GameObject floorObject = floorTransform.gameObject;
                            floorObject.layer = LayerDungeon;
                        }
                    }
                }
            }
        }
    }





    void SpawnEnemyInRoom(GameObject currentRoom)
    {
        int numberOfEnemies = Random.Range(1, 3);

        Tilemap tilemap = currentRoom.GetComponentInChildren<Tilemap>();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3Int randomCell = new Vector3Int(
                Random.Range(tilemap.cellBounds.xMin + 3, tilemap.cellBounds.xMax - 3),
                Random.Range(tilemap.cellBounds.yMin + 3, tilemap.cellBounds.yMax - 3),
                0
            );

            Vector3 spawnPosition = tilemap.CellToWorld(randomCell);

            GameObject newEnemy = Instantiate(meleEnemyPrefab, spawnPosition, Quaternion.identity);

            if (enemiesInRoom.ContainsKey(currentRoom))
            {
                enemiesInRoom[currentRoom]++;
            }
            else
            {
                enemiesInRoom[currentRoom] = 1;
            }

            MeleEnemyAI enemyAI = newEnemy.GetComponent<MeleEnemyAI>();

            if (enemyAI != null)
            {
                enemyAI.currentRoom = currentRoom;
                enemyAI.dungeonGenerator = this;
                enemyAI.player = player.transform;
            }
        }
    }    

    public void DecreaseEnemiesInRoom(GameObject currentRoom)
    {
        if (enemiesInRoom.ContainsKey(currentRoom))
        {
            enemiesInRoom[currentRoom]--;
        }

        if(enemiesInRoom[currentRoom] <= 0)
        {
            CheckRoomsPositions();
        }
    }


    void CheckRoomsPositions()
    {

        if (player == null)
        {
            Debug.LogWarning("Brak obiektu gracza.");
            return;
        }

        GameObject currentRoom = null;

        foreach (GameObject room in generatedRooms)
        {
            BoxCollider2D floorCollider = room.GetComponentInChildren<BoxCollider2D>();

            if (floorCollider != null && floorCollider.bounds.Contains(player.transform.position))
            {
                currentRoom = room;
                break;
            }

        }

        if (currentRoom == null)
        {
            Debug.LogWarning("Gracz nie znajduje siê w ¿adnym pomieszczeniu.");
            return;
        }

        Vector2[] CheckDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        if (enemiesInRoom.ContainsKey(currentRoom) && enemiesInRoom[currentRoom] <= 0)
        {
            foreach (Vector2 direction in CheckDirections)
            {
                float maxDistance = direction == Vector2.up || direction == Vector2.down ? MaxConDistanceY : MaxConDistanceX;

                BoxCollider2D floorCollider = currentRoom.GetComponentInChildren<BoxCollider2D>();

                if (floorCollider != null)
                {
                    Vector2 raycastOrigin = currentRoom.GetComponentInChildren<Grid>().transform.position;

                    RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, direction, maxDistance, roomLayer);

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.collider.CompareTag("Floor") && hit.collider.gameObject != currentRoom)
                        {
                            GameObject connectedRoom = hit.collider.gameObject;

                            if (direction == Vector2.up)
                            {
                                Transform gridTransform = currentRoom.transform.Find("Grid");

                                if (gridTransform != null)
                                {
                                    Transform wallsTransform = gridTransform.Find("Walls");

                                    if (wallsTransform != null)
                                    {
                                        Tilemap wallsTilemap = wallsTransform.GetComponent<Tilemap>();
                                        wallsTilemap.SetTile(coordsDoorsUp1, doorUp1);
                                        wallsTilemap.SetTile(coordsDoorsUp2, doorUp2);
                                    }
                                }
                                GameObject UpLevelChanger = Instantiate(ChangeLevelUp, currentRoom.transform.position + new Vector3(0, 3.9f, 0), transform.rotation);
                            }
                            else if (direction == Vector2.down)
                            {
                                Transform gridTransform = currentRoom.transform.Find("Grid");

                                if (gridTransform != null)
                                {
                                    Transform wallsTransform = gridTransform.Find("Walls");

                                    if (wallsTransform != null)
                                    {
                                        Tilemap wallsTilemap = wallsTransform.GetComponent<Tilemap>();
                                        wallsTilemap.SetTile(coordsDoorsDown1, doorDown1);
                                        wallsTilemap.SetTile(coordsDoorsDown2, doorDown2);
                                    }
                                }
                                GameObject DownLevelChanger = Instantiate(ChangeLevelDown, currentRoom.transform.position - new Vector3(0, 3.9f, 0), transform.rotation);
                            }
                            else if (direction == Vector2.right)
                            {
                                Transform gridTransform = currentRoom.transform.Find("Grid");

                                if (gridTransform != null)
                                {
                                    Transform wallsTransform = gridTransform.Find("Walls");

                                    if (wallsTransform != null)
                                    {
                                        Tilemap wallsTilemap = wallsTransform.GetComponent<Tilemap>();
                                        wallsTilemap.SetTile(coordsDoorsRight1, doorRight1);
                                        wallsTilemap.SetTile(coordsDoorsRight2, doorRight2);
                                    }
                                }
                                GameObject LeftLevelChanger = Instantiate(ChangeLevelRight, currentRoom.transform.position + new Vector3(8.9f, 0, 0), transform.rotation);
                            }
                            else if (direction == Vector2.left)
                            {
                                Transform gridTransform = currentRoom.transform.Find("Grid");

                                if (gridTransform != null)
                                {
                                    Transform wallsTransform = gridTransform.Find("Walls");

                                    if (wallsTransform != null)
                                    {
                                        Tilemap wallsTilemap = wallsTransform.GetComponent<Tilemap>();
                                        wallsTilemap.SetTile(coordsDoorsLeft1, doorLeft1);
                                        wallsTilemap.SetTile(coordsDoorsLeft2, doorLeft2);
                                    }
                                }
                                GameObject LeftLevelChanger = Instantiate(ChangeLevelLeft, currentRoom.transform.position - new Vector3(8.9f, 0, 0), transform.rotation);
                            }
                        }
                    }
                }
            }
        }
    }
}
