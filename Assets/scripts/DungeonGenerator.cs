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
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Dictionary<GameObject, bool> roomsSpawned = new Dictionary<GameObject, bool>();
    public LayerMask roomLayer;
    private float MaxConDistanceX = 25f;
    private float MaxConDistanceY = 15f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                        Debug.Log("Gracz znajduje siê w pomieszczeniu: " + room.name);
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

        for (int i = 0; i < 1; i++)
        {
            Vector3Int randomCell = new Vector3Int(
                Random.Range(tilemap.cellBounds.xMin + 3, tilemap.cellBounds.xMax - 3),
                Random.Range(tilemap.cellBounds.yMin + 3, tilemap.cellBounds.yMax - 3),
                0
            );

            Vector3 spawnPosition = tilemap.CellToWorld(randomCell);

            GameObject newEnemy = Instantiate(meleEnemyPrefab, spawnPosition, Quaternion.identity);
            MeleEnemyAI enemyAI = newEnemy.GetComponent<MeleEnemyAI>();

            if (enemyAI != null)
            {
                //enemyAI.player = player.transform;
            }
        }
    }

    void TrackEnemiesInRoom(GameObject currentRoom)
    {

    }

    void CheckRoomsPositions()
    {
        if (player == null)
        {
            Debug.LogWarning("Brak obiektu gracza.");
            return;
        }

        GameObject currentRoom = null;

        // ZnajdŸ pomieszczenie, w którym aktualnie znajduje siê gracz
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
                        Debug.Log(hit.point);
                        if (!generatedRooms.Contains(connectedRoom))
                        {
                            generatedRooms.Add(connectedRoom);

                            if (direction == Vector2.up)
                            {
                                Debug.Log("Up: Hit object: " + connectedRoom.name);
                            }
                            else if (direction == Vector2.down)
                            {
                                Debug.Log("Down: Hit object: " + connectedRoom.name);
                            }
                            else if (direction == Vector2.right)
                            {
                                Debug.Log("Right: Hit object: " + connectedRoom.name);
                            }
                            else if (direction == Vector2.left)
                            {
                                Debug.Log("Left: Hit object: " + connectedRoom.name);
                            }
                        }
                    }
                }
            }
        }

    }


}
