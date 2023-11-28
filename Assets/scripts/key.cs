using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class key : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase nowyKafel;
    public Vector3Int wspolrzedneKafelka1;
    public Vector3Int wspolrzedneKafelka2;

    public GameObject changelevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemap.SetTile(wspolrzedneKafelka1, nowyKafel);
            tilemap.SetTile(wspolrzedneKafelka2, nowyKafel);

            GameObject levelChanger = Instantiate(changelevel, new Vector2(0, 3.5f), transform.rotation);

            Destroy(gameObject);
        }
    }
}
