using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int numberOfRooms = 25;
    public float roomDistanceX = 25f;
    public float roomDistanceY = 15f;
    float tolerance = 0.1f;

    private GameObject lastRoom;
    public GameObject startingRoom;

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

        Vector3 spawnPosition = startingRoom.transform.position;

        lastRoom = startingRoom;

        List<Vector2> directions = new List<Vector2> { Vector2.right, Vector2.up, };

        for (int i = 0; i < numberOfRooms; i++)
        {
            int numberOfRoomsToSpawn = Random.Range(1, 3); // Zawsze 2 lub 3 pomieszczenia obok siebie

            for (int j = 0; j < numberOfRoomsToSpawn; j++)
            {
                Vector2 direction = directions[Random.Range(0, directions.Count)];
                Vector3 newPosition = spawnPosition + new Vector3(direction.x * roomDistanceX, direction.y * roomDistanceY, 0);

                // Sprawdzenie, czy nowa pozycja nie jest taka sama jak poprzednia
                if (lastRoom != null && Vector3.Distance(newPosition, lastRoom.transform.position) > tolerance)
                {
                    GameObject newRoom = Instantiate(roomPrefab, newPosition, Quaternion.identity);
                    lastRoom = newRoom;
                }
                else
                {
                    // Jeœli nowa pozycja jest taka sama jak poprzednia, przesuñ siê jeszcze raz w tym samym kierunku
                    spawnPosition += new Vector3(direction.x * roomDistanceX, direction.y * roomDistanceY, 0);
                    GameObject newRoom = Instantiate(roomPrefab, spawnPosition, Quaternion.identity);
                    lastRoom = newRoom;
                }
            }
        }
    }
}
