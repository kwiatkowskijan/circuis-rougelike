using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemiesSpawner : MonoBehaviour
{

    public GameObject enemy1;
    public GameObject enemy2;
    public Tilemap tilemap;
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        for (int i = 0; i < 3; i++)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("DUPA");

                Vector3Int randomCell = new Vector3Int(
                    Random.Range(tilemap.cellBounds.xMin + 2, tilemap.cellBounds.xMax - 2),
                    Random.Range(tilemap.cellBounds.yMin + 2, tilemap.cellBounds.yMax - 2),
                    0
                );

                Vector3 spawnPosition = tilemap.CellToWorld(randomCell);


                Instantiate(enemy1, spawnPosition, Quaternion.identity);

            }
        }
     
    }
}
