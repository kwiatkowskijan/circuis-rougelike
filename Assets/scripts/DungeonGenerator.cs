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

    public Tilemap wallsTilemap;
    public Vector3Int coordsDoorsUp1;
    public Vector3Int coordsDoorsUp2;
    public Vector3Int coordsDoorsLeft1;
    public Vector3Int coordsDoorsLeft2;
    public Vector3Int coordsDoorsDown1;
    public Vector3Int coordsDoorsDown2;
    public Vector3Int coordsDoorsRight1;
    public Vector3Int coordsDoorsRight2;
    public TileBase doorUp1;
    public TileBase doorUp2;
    public TileBase doorLeft1;
    public TileBase doorLeft2;
    public TileBase doorDown1;
    public TileBase doorDown2;
    public TileBase doorRight1;
    public TileBase doorRight2;

    public GameObject ChangeLevelUp;
    public GameObject ChangeLevelLeft;
    public GameObject ChangeLevelRight;
    public GameObject ChangeLevelDown;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wallsTilemap = roomPrefab.GetComponentInChildren<Tilemap>();
        GenerateDungeon();
    }

    void Update()
    {
        CheckPlayerRoom();
        CheckRoomsPositions();
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

        if (player != null)
        {
            foreach (GameObject room in generatedRooms)
            {
                BoxCollider2D floorCollider = room.GetComponentInChildren<BoxCollider2D>();

                if (floorCollider != null)
                {
                    if (floorCollider.bounds.Contains(player.transform.position))
                    {
                        //Debug.Log("Gracz znajduje siê w pomieszczeniu: " + room.name);
                        TrackEnemiesInRoom(room);

                        if (!roomsSpawned.ContainsKey(room) || !roomsSpawned[room])
                        {
                            SpawnEnemyInRoom(room);
                            roomsSpawned[room] = true;
                        }
                        break;
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

            Debug.Log("Enemies in " + currentRoom.name + ": " + enemiesInRoom[currentRoom]);

            if (enemyAI != null)
            {
                enemyAI.currentRoom = currentRoom;
                enemyAI.dungeonGenerator = this;
                //enemyAI.player = player.transform;

                Debug.Log("Enemies in " + currentRoom.name + ": " + enemiesInRoom[currentRoom]);
            }
        }
    }


    void TrackEnemiesInRoom(GameObject currentRoom)
    {
        if (enemiesInRoom.ContainsKey(currentRoom))
        {
            //Debug.Log("Enemies in " + currentRoom.name + ": " + enemiesInRoom[currentRoom]);

            MeleEnemyAI[] enemies = currentRoom.GetComponentsInChildren<MeleEnemyAI>();

            foreach (MeleEnemyAI enemy in enemies)
            {
                if (enemy.currentHealth <= 0)
                {
                    Debug.Log("Enemy killed in " + currentRoom.name);
                    enemiesInRoom[currentRoom]--;
                }
            }

            //Debug.Log("Remaining enemies in " + currentRoom.name + ": " + enemiesInRoom[currentRoom]);
        }
        else
        {
            //Debug.Log("No enemies in " + currentRoom.name);
        }
    }

    public void DecreaseEnemiesInRoom(GameObject currentRoom)
    {
        if (enemiesInRoom.ContainsKey(currentRoom))
        {
            enemiesInRoom[currentRoom]--;
            //Debug.Log("Remaining enemies in " + currentRoom.name + ": " + enemiesInRoom[currentRoom]);
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

                    Debug.DrawRay(currentRoom.transform.position, direction * maxDistance, Color.blue, 0.1f);

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.collider.CompareTag("Floor") && hit.collider.gameObject != currentRoom)
                        {
                            GameObject connectedRoom = hit.collider.gameObject;

                            if (!generatedRooms.Contains(connectedRoom))
                            {
                                generatedRooms.Add(connectedRoom);

                                if (direction == Vector2.up)
                                {
                                    Debug.Log("Up: Hit object: " + connectedRoom.name);
                                    //wallsTilemap.SetTile(coordsDoorsUp1, doorUp2);
                                    //wallsTilemap.SetTile(coordsDoorsUp2, doorUp1);
                                    GameObject UpLevelChanger = Instantiate(ChangeLevelUp, currentRoom.transform.position + new Vector3(0, 3.9f, 0), transform.rotation);
                                }
                                else if (direction == Vector2.down)
                                {
                                    Debug.Log("Down: Hit object: " + connectedRoom.name);
                                    GameObject DownLevelChanger = Instantiate(ChangeLevelDown, currentRoom.transform.position - new Vector3(0, 3.9f, 0), transform.rotation);
                                }
                                else if (direction == Vector2.right)
                                {
                                    Debug.Log("Right: Hit object: " + connectedRoom.name);
                                    GameObject LeftLevelChanger = Instantiate(ChangeLevelRight, currentRoom.transform.position + new Vector3(8.9f, 0, 0), transform.rotation);
                                }
                                else if (direction == Vector2.left)
                                {
                                    Debug.Log("Left: Hit object: " + connectedRoom.name);
                                    GameObject LeftLevelChanger = Instantiate(ChangeLevelLeft, currentRoom.transform.position - new Vector3(8.9f, 0, 0), transform.rotation);
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
