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
                        SpawnEnemyInRoom();
                        break;
                    }
                }
            }
        }
    }

    void SpawnEnemyInRoom()
    {
        int EnemiesToSpawn = 3;

        foreach (GameObject room in generatedRooms)
        {
            Tilemap[] tilemaps = room.GetComponentsInChildren<Tilemap>();

            foreach (Tilemap tilemap in tilemaps)
            {
                // SprawdŸ, czy Tilemapa ma odpowiedni¹ nazwê
                if (tilemap.name == "floor")
                {
                    for (int i = 0; i < EnemiesToSpawn; i++)
                    {
                        Vector3Int randomCell = new Vector3Int(
                            Random.Range(tilemap.cellBounds.xMin + 3, tilemap.cellBounds.xMax - 3),
                            Random.Range(tilemap.cellBounds.yMin + 3, tilemap.cellBounds.yMax - 3),
                            0
                        );

                        Vector3 spawnPosition = tilemap.CellToWorld(randomCell);

                        GameObject newEnemy = Instantiate(meleEnemyPrefab, spawnPosition, Quaternion.identity);
                        spawnedEnemies.Add(newEnemy);
                    }

                    // Je¿eli znaleziono Tilemapê o odpowiedniej nazwie, przerwij pêtlê
                    break;
                }
            }
        }
    }

}
