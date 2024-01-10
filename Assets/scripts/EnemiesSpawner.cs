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

    public LayerMask roomLayer;
    private float MaxConDistanceX = 25f;
    private float MaxConDistanceY = 15f;

    public Tilemap wallsTilemap;
    public TileBase doorUp1;
    public TileBase doorUp2;
    public Vector3Int coordsDoorsUp1;
    public Vector3Int coordsDoorsUp2;

    private void Start()
    {
        GameObject room = GetComponent<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 

            MeleEnemyAI[] enemies = GetComponentsInChildren<MeleEnemyAI>();

            foreach (MeleEnemyAI enemyAI in enemies)
            {
                enemyAI.player = collision.transform;
            }

            //Invoke("SpawnEnemy", spawnDelay);
            CheckRoomsPositions();


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

            GameObject newEnemy = Instantiate(enemy1, spawnPosition, Quaternion.identity);
            MeleEnemyAI enemyAI = newEnemy.GetComponent<MeleEnemyAI>();

            if (enemyAI != null)
            {
                enemyAI.player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }

    void CheckRoomsPositions()
    {
        List<GameObject> connectedRooms = new List<GameObject>();
        Vector2[] CheckDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 direction in CheckDirections)
        {
            float maxDistance = direction == Vector2.up || direction == Vector2.down ? MaxConDistanceY : MaxConDistanceX;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, maxDistance, roomLayer);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.CompareTag("Floor")) 
                {
                    GameObject connectedRoom = hit.collider.gameObject;
                    if (!connectedRooms.Contains(connectedRoom))
                    {
                        connectedRooms.Add(connectedRoom);

                        if (direction == Vector2.up)
                        {
                            wallsTilemap.SetTile(coordsDoorsUp1, doorUp2);
                            wallsTilemap.SetTile(coordsDoorsUp2, doorUp1);
                        }
                        else if (direction == Vector2.down)
                        {
                   
                        }
                        else if (direction == Vector2.right)
                        {
                            
                        }
                        else if (direction == Vector2.left)
                        {
                            
                        }
                    }
                }
            }
        }
    }
   
}
