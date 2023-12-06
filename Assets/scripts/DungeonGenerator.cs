using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject startingRoom;
    public int numberOfRooms = 15;
    public float roomDistanceX = 25f;
    public float roomDistanceY = 15f;
    public int maxRoomsPerIteration = 4;

    private List<GameObject> generatedRooms = new List<GameObject>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        if (startingRoom == null)
        {
            Debug.LogWarning("Brak pocz¹tkowego pomieszczenia do generowania kolejnych.");
            return;
        }

        generatedRooms.Add(startingRoom); // Dodanie pocz¹tkowego pomieszczenia

        int generatedCount = 0;
        while (generatedCount < numberOfRooms - 1) // -1 dla pominiêcia pomieszczenia pocz¹tkowego
        {
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
            Vector2 randomDirection = directions[Random.Range(0, directions.Length)];

            Vector3 newPosition = generatedRooms[Random.Range(0, generatedRooms.Count)].transform.position +
                new Vector3(randomDirection.x * roomDistanceX, randomDirection.y * roomDistanceY, 0f);

            bool positionOccupied = IsPositionOccupied(newPosition);

            if (!positionOccupied)
            {
                GameObject newRoom = Instantiate(roomPrefab, newPosition, Quaternion.identity);
                generatedRooms.Add(newRoom); // Dodanie nowego pomieszczenia do listy
                generatedCount++;
            }
        }

        bool IsPositionOccupied(Vector3 position)
        {
            foreach (GameObject room in generatedRooms)
            {
                if (Vector3.Distance(room.transform.position, position) < 0.1f) // Sprawdzenie odleg³oœci
                {
                    return true;
                }
            }
            return false;
        }
    }
}
