using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public Tilemap tilemap;
    public float spawnDelay = 2.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("SpawnEnemy", spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < Random.Range(1, 10); i++)
        {
            Vector3Int randomCell = new Vector3Int(
                Random.Range(tilemap.cellBounds.xMin + 3, tilemap.cellBounds.xMax - 3),
                Random.Range(tilemap.cellBounds.yMin + 3, tilemap.cellBounds.yMax - 3),
                0
            );

            Vector3 spawnPosition = tilemap.CellToWorld(randomCell);

            Instantiate(enemy1, spawnPosition, Quaternion.identity);
        }
    }
}
