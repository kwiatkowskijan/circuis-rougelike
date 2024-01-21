using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class key : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase doorUp1;
    public TileBase doorUp2;
    public Vector3Int wspolrzedneKafelkaUp1;
    public Vector3Int wspolrzedneKafelkaUp2;

    public GameObject changelevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemap.SetTile(wspolrzedneKafelkaUp1, doorUp1);
            tilemap.SetTile(wspolrzedneKafelkaUp2, doorUp2);

            GameObject levelChanger = Instantiate(changelevel, new Vector2(0, 3.9f), transform.rotation);

            Destroy(gameObject);
        }
    }
}
